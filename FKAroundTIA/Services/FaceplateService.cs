using FKAroundTIA.Models;
using Siemens.Engineering;
using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HmiUnified.UI.Controls;
using Siemens.Engineering.HmiUnified.UI.Dynamization;
using Siemens.Engineering.HmiUnified.UI.Screens;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FKAroundTIA.Services
{
    public class FaceplateService
    {
        private readonly ScreenService _screenService = new ScreenService();

        public IList<FaceplateInstanceInfo> GetFaceplates(HmiScreen screen, HmiSoftware hmiSoftware, bool includeReferencedScreens = true)
        {
            var list = new List<FaceplateInstanceInfo>();
            if (screen == null || hmiSoftware == null) return list;

            ScanScreen(screen, hmiSoftware, screen.Name, list, includeReferencedScreens);
            return list;
        }

        private void ScanScreen(
            HmiScreen screen,
            HmiSoftware hmiSoftware,
            string currentPath,
            List<FaceplateInstanceInfo> list,
            bool includeReferencedScreens)
        {
            if (screen == null || screen.ScreenItems == null) return;

            foreach (var item in screen.ScreenItems)
            {
                if (item is HmiFaceplateContainer faceplate)
                {
                    // Traverse parent chain within the screen to find groups
                    string groupPath = "";
                    string parentGroupName = "";
                    IEngineeringObject parent = faceplate.Parent;

                    while (parent != null && parent != screen)
                    {
                        string name = GetObjectName(parent);
                        if (!string.IsNullOrEmpty(name))
                        {
                            groupPath = name + (string.IsNullOrEmpty(groupPath) ? "" : "/" + groupPath);
                            if (string.IsNullOrEmpty(parentGroupName))
                            {
                                parentGroupName = name;
                            }
                        }
                        parent = parent.Parent;
                    }

                    string fullPath = currentPath;
                    if (!string.IsNullOrEmpty(groupPath))
                    {
                        fullPath += "/" + groupPath;
                    }
                    fullPath += "/" + faceplate.Name;

                    list.Add(new FaceplateInstanceInfo
                    {
                        Name = faceplate.Name,
                        FaceplateTypeName = faceplate.ContainedType,
                        ParentGroup = parentGroupName,
                        FullPath = fullPath,
                        X = faceplate.Left,
                        Y = faceplate.Top,
                        Width = (int)faceplate.Width,
                        Height = (int)faceplate.Height,
                        Container = faceplate
                    });
                }
                else if (includeReferencedScreens && item is HmiScreenWindow screenWindow)
                {
                    string targetScreenName = screenWindow.Screen;
                    if (!string.IsNullOrEmpty(targetScreenName))
                    {
                        var targetScreen = _screenService.FindScreenRecursive(hmiSoftware, targetScreenName);
                        if (targetScreen != null)
                        {
                            string nestedPath = currentPath + "/" + screenWindow.Name;
                            ScanScreen(targetScreen, hmiSoftware, nestedPath, list, true);
                        }
                    }
                }
            }
        }

        public IList<FaceplateInterfaceItemInfo> GetInterface(HmiFaceplateContainer container)
        {
            if (container?.Interface == null) return new List<FaceplateInterfaceItemInfo>();

            return container.Interface.Select(item =>
            {
                var tagDyn = item.Dynamizations?.OfType<TagDynamization>().FirstOrDefault();

                return new FaceplateInterfaceItemInfo(
                    item.PropertyName,
                    tagDyn?.DataType ?? item.Value?.GetType().Name ?? "",
                    item.Value,
                    tagDyn?.Tag ?? tagDyn?.PlcTag ?? ""
                );
            }).ToList();
        }

        private string GetObjectName(IEngineeringObject obj)
        {
            if (obj == null) return "";
            try
            {
                PropertyInfo prop = obj.GetType().GetProperty("Name");
                if (prop != null)
                {
                    return prop.GetValue(obj) as string;
                }
            }
            catch
            {
                // Ignore reflection errors
            }
            return "";
        }
    }
}
