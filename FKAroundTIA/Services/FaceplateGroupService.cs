using FKAroundTIA.Models;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HmiUnified.UI.Controls;
using Siemens.Engineering.HmiUnified.UI.Dynamization;
using Siemens.Engineering.HmiUnified.UI.Parts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FKAroundTIA.Services
{
    public class FaceplateGroupService
    {
        private readonly RuntimeInfo _runtime;
        private readonly FaceplateService _faceplateService = new FaceplateService();
        private readonly ProjectTextService _projectTextService = new ProjectTextService();
        private readonly ScreenService _screenService = new ScreenService();

        public FaceplateGroupService(RuntimeInfo runtime)
        {
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
        }

        public IList<ScreenFaceplateInfo> GetFaceplateGroupsByScreen()
        {
            if (_runtime.DeviceItem == null)
            {
                return new List<ScreenFaceplateInfo>();
            }

            var container = _runtime.DeviceItem.GetService<SoftwareContainer>();
            var hmiSoftware = container?.Software as HmiSoftware;
            if (hmiSoftware == null)
            {
                throw new InvalidOperationException("Failed to access HmiSoftware services.");
            }

            var screens = _screenService.GetAllScreens(hmiSoftware);
            return screens
                .Select(screen => new ScreenFaceplateInfo
                {
                    Name = screen.Name,
                    Groups = _faceplateService
                        .GetFaceplates(screen, hmiSoftware, false)
                        .GroupBy(faceplate => faceplate.FaceplateTypeName)
                        .Select(group => new FaceplateTypeGroupInfo
                        {
                            TypeName = group.Key,
                            Count = group.Count(),
                            Instances = group.ToList()
                        })
                        .OrderBy(group => group.TypeName)
                        .ToList()
                })
                .Where(screen => screen.InstanceCount > 0)
                .ToList();
        }

        public System.Data.DataTable GetInterfacePropertiesTable(FaceplateTypeGroupInfo group)
        {
            var table = new System.Data.DataTable();
            table.Columns.Add("Faceplate Name", typeof(string));

            if (group == null || group.Instances == null || group.Instances.Count == 0)
            {
                return table;
            }

            // Get property names from the first found container
            var firstContainer = group.Instances
                .Select(instance => instance.Container)
                .FirstOrDefault(container => container?.Interface != null);
            if (firstContainer?.Interface == null) return table;

            var propNames = new List<string>();
            foreach (var item in firstContainer.Interface)
            {
                table.Columns.Add(item.PropertyName, typeof(string));
                propNames.Add(item.PropertyName);
            }

            // Populate rows for each instance
            foreach (var instance in group.Instances)
            {
                var row = table.NewRow();
                row["Faceplate Name"] = instance.Name;

                if (instance.Container?.Interface != null)
                {
                    foreach (var propName in propNames)
                    {
                        var fpProp = instance.Container.Interface.Find(propName);
                        if (fpProp != null)
                        {
                            var tagDyn = fpProp.Dynamizations?.OfType<TagDynamization>().FirstOrDefault();
                            if (tagDyn != null)
                            {
                                row[propName] = tagDyn.Tag ?? tagDyn.PlcTag ?? "";
                            }
                            else
                            {
                                string value = fpProp.Value?.ToString() ?? "";
                                row[propName] = _projectTextService.TryGetPlainText(value, out var plainText)
                                    ? plainText
                                    : value;
                            }
                        }
                        else
                        {
                            row[propName] = "";
                        }
                    }
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public void ApplyChangesToAll(FaceplateTypeGroupInfo group, IDictionary<string, IDictionary<string, string>> valuesByInstanceName)
        {
            if (group == null || group.Instances == null) return;
            if (valuesByInstanceName == null || valuesByInstanceName.Count == 0) return;

            foreach (var instance in group.Instances)
            {
                if (instance.Container?.Interface == null)
                {
                    continue;
                }

                if (!valuesByInstanceName.TryGetValue(instance.Name, out var propertyValues))
                {
                    continue;
                }

                foreach (var propertyValue in propertyValues)
                {
                    var fpProp = instance.Container.Interface.Find(propertyValue.Key);
                    if (fpProp != null)
                    {
                        ApplyPropertyChange(fpProp, propertyValue.Value);
                    }
                }
            }
        }

        private void ApplyPropertyChange(HmiFaceplateInterface fpProp, string newValue)
        {
            string currentValue = fpProp.Value?.ToString();
            var currentTagDynamization = fpProp.Dynamizations?.OfType<TagDynamization>().FirstOrDefault();
            if (currentTagDynamization == null
                && _projectTextService.TryGetPlainText(currentValue, out var currentPlainText))
            {
                if (string.Equals(currentPlainText, newValue ?? "", StringComparison.Ordinal))
                {
                    return;
                }

                DeleteDynamizationIfExists(fpProp);
                fpProp.Value = _projectTextService.UpdatePlainText(currentValue, newValue);
                return;
            }

            if (string.IsNullOrEmpty(newValue))
            {
                DeleteDynamizationIfExists(fpProp);
                try { fpProp.Value = null; } catch {}
                return;
            }

            // Try parsing as boolean
            if (bool.TryParse(newValue, out bool boolVal))
            {
                DeleteDynamizationIfExists(fpProp);
                try { fpProp.Value = boolVal; } catch {}
                return;
            }

            // Try parsing as Color (must be before int/double to avoid partial matches)
            var color = TryParseColor(newValue);
            if (color.HasValue)
            {
                DeleteDynamizationIfExists(fpProp);
                try { fpProp.Value = color.Value; } catch {}
                return;
            }

            // Try parsing as integer
            if (int.TryParse(newValue, out int intVal))
            {
                DeleteDynamizationIfExists(fpProp);
                try { fpProp.Value = intVal; } catch {}
                return;
            }

            // Try parsing as double
            if (double.TryParse(newValue, out double doubleVal))
            {
                DeleteDynamizationIfExists(fpProp);
                try { fpProp.Value = doubleVal; } catch {}
                return;
            }

            // Otherwise treat it as a Tag Binding
            var tagDyn = fpProp.Dynamizations?.OfType<TagDynamization>().FirstOrDefault();
            if (tagDyn == null)
            {
                try
                {
                    tagDyn = fpProp.Dynamizations.Create<TagDynamization>("Value");
                }
                catch
                {
                    // Fallback
                }
            }

            if (tagDyn != null)
            {
                try
                {
                    tagDyn.Tag = newValue;
                }
                catch
                {
                    // If tag assignment fails (e.g. invalid tag name in TIA), write it as a constant string value
                    DeleteDynamizationIfExists(fpProp);
                    try { fpProp.Value = newValue; } catch {}
                }
            }
            else
            {
                // Fallback: write as constant string
                try { fpProp.Value = newValue; } catch {}
            }
        }

        /// <summary>
        /// Tries to parse a color string in various formats:
        ///   "Color [A=255, R=128, G=0, B=64]"  (from Color.ToString())
        ///   "#AARRGGBB" or "#RRGGBB"            (hex)
        ///   "255,128,0,64" or "128,0,64"        (A,R,G,B or R,G,B)
        ///   Named colors like "Red", "Blue"
        /// </summary>
        private System.Drawing.Color? TryParseColor(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var trimmed = value.Trim();

            // Format: "Color [A=255, R=128, G=0, B=64]"
            if (trimmed.StartsWith("Color [", StringComparison.OrdinalIgnoreCase) && trimmed.EndsWith("]"))
            {
                try
                {
                    var inner = trimmed.Substring(7, trimmed.Length - 8); // strip "Color [" and "]"
                    int a = 255, r = 0, g = 0, b = 0;
                    foreach (var part in inner.Split(','))
                    {
                        var kv = part.Trim().Split('=');
                        if (kv.Length != 2) continue;
                        int val = int.Parse(kv[1].Trim());
                        switch (kv[0].Trim().ToUpperInvariant())
                        {
                            case "A": a = val; break;
                            case "R": r = val; break;
                            case "G": g = val; break;
                            case "B": b = val; break;
                        }
                    }
                    return System.Drawing.Color.FromArgb(a, r, g, b);
                }
                catch { return null; }
            }

            // Format: "#AARRGGBB" or "#RRGGBB"
            if (trimmed.StartsWith("#"))
            {
                try
                {
                    return System.Drawing.ColorTranslator.FromHtml(trimmed);
                }
                catch { return null; }
            }

            // Format: "R,G,B" or "A,R,G,B"
            var parts = trimmed.Split(',');
            if (parts.Length == 3 || parts.Length == 4)
            {
                bool allInts = true;
                var vals = new int[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                {
                    if (!int.TryParse(parts[i].Trim(), out vals[i]) || vals[i] < 0 || vals[i] > 255)
                    {
                        allInts = false;
                        break;
                    }
                }
                if (allInts)
                {
                    return parts.Length == 4
                        ? System.Drawing.Color.FromArgb(vals[0], vals[1], vals[2], vals[3])
                        : System.Drawing.Color.FromArgb(vals[0], vals[1], vals[2]);
                }
            }

            // Try named color (e.g., "Red", "Blue", "Transparent")
            try
            {
                var namedColor = System.Drawing.Color.FromName(trimmed);
                if (namedColor.IsKnownColor) return namedColor;
            }
            catch { }

            return null;
        }

        private void DeleteDynamizationIfExists(HmiFaceplateInterface fpProp)
        {
            var tagDyn = fpProp.Dynamizations?.OfType<TagDynamization>().FirstOrDefault();
            if (tagDyn != null)
            {
                try { tagDyn.Delete(); } catch {}
            }
        }
    }
}
