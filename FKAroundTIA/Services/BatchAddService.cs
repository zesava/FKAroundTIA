using FKAroundTIA.Models;
using Siemens.Engineering;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HmiUnified.UI.Controls;
using Siemens.Engineering.HmiUnified.UI.Screens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FKAroundTIA.Services
{
    public class BatchAddService
    {
        private const string TemplateScreenName = "tpl";

        private readonly RuntimeInfo _runtime;
        private readonly TiaPortal _tiaPortal;
        private readonly Project _project;
        private readonly ScreenService _screenService = new ScreenService();
        private readonly FaceplateService _faceplateService = new FaceplateService();
        private readonly ProjectTextService _projectTextService = new ProjectTextService();

        public BatchAddService(RuntimeInfo runtime, TiaPortal tiaPortal, Project project)
        {
            _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
            _tiaPortal = tiaPortal ?? throw new ArgumentNullException(nameof(tiaPortal));
            _project = project ?? throw new ArgumentNullException(nameof(project));
        }

        public IList<BatchAddFaceplateTypeInfo> GetFaceplateTypes()
        {
            HmiSoftware hmiSoftware = GetHmiSoftware();
            HmiScreen templateScreen = _screenService
                .GetAllScreens(hmiSoftware)
                .FirstOrDefault(screen => string.Equals(
                    screen.Name,
                    TemplateScreenName,
                    StringComparison.OrdinalIgnoreCase));

            if (templateScreen == null)
            {
                throw new InvalidOperationException(
                    $"Template screen '{TemplateScreenName}' was not found in the selected Runtime.");
            }

            var templates = _faceplateService.GetFaceplates(templateScreen, hmiSoftware, false);
            if (templates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Template screen '{templateScreen.Name}' does not contain any faceplate instances.");
            }

            return templates
                .Where(item => !string.IsNullOrWhiteSpace(item.FaceplateTypeName))
                .GroupBy(item => item.FaceplateTypeName, StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .Select(item => new BatchAddFaceplateTypeInfo
                {
                    TypeName = item.FaceplateTypeName,
                    Width = item.Width,
                    Height = item.Height,
                    TemplateContainer = item.Container
                })
                .OrderBy(item => item.TypeName)
                .ToList();
        }

        public IList<BatchAddScreenInfo> GetScreens()
        {
            return _screenService
                .GetAllScreens(GetHmiSoftware())
                .Select(screen => new BatchAddScreenInfo
                {
                    Name = screen.Name,
                    Width = (int)screen.Width,
                    Height = (int)screen.Height,
                    Screen = screen
                })
                .OrderBy(screen => screen.Name)
                .ToList();
        }

        public int AddFaceplates(BatchAddRequest request)
        {
            ValidateRequest(request);

            IList<string> names = BuildNames(request);
            IList<GridPosition> positions = BuildGridPositions(request);
            ValidateNamesAreAvailable(request.TargetScreen.Screen, names);

            string templateLabelValue = GetTemplateLabelValue(request.FaceplateType.TemplateContainer);

            using (ExclusiveAccess exclusiveAccess = _tiaPortal.ExclusiveAccess("Batch adding faceplates"))
            using (Transaction transaction = exclusiveAccess.Transaction(_project, "Batch add faceplates"))
            {
                for (int index = 0; index < names.Count; index++)
                {
                    string instanceName = names[index];
                    GridPosition position = positions[index];

                    var container = request.TargetScreen.Screen.ScreenItems
                        .Create<HmiFaceplateContainer>(instanceName, request.FaceplateType.TypeName);

                    container.Left = position.X;
                    container.Top = position.Y;
                    container.Width = (uint)request.FaceplateType.Width;
                    container.Height = (uint)request.FaceplateType.Height;

                    var label = container.Interface?.Find("Label");
                    if (label == null)
                    {
                        throw new InvalidOperationException(
                            $"Faceplate type '{request.FaceplateType.TypeName}' does not expose a 'Label' interface property.");
                    }

                    label.Value = _projectTextService.TryGetPlainText(templateLabelValue, out _)
                        ? _projectTextService.UpdatePlainText(templateLabelValue, instanceName)
                        : instanceName;
                }

                transaction.CommitOnDispose();
            }

            return names.Count;
        }

        private HmiSoftware GetHmiSoftware()
        {
            var softwareContainer = _runtime.DeviceItem?.GetService<SoftwareContainer>();
            var hmiSoftware = softwareContainer?.Software as HmiSoftware;
            if (hmiSoftware == null)
            {
                throw new InvalidOperationException("Failed to access HmiSoftware services.");
            }

            return hmiSoftware;
        }

        private static void ValidateRequest(BatchAddRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.FaceplateType?.TemplateContainer == null)
                throw new InvalidOperationException("Select a faceplate type.");
            if (request.TargetScreen?.Screen == null)
                throw new InvalidOperationException("Select a target screen.");
            if (string.IsNullOrWhiteSpace(request.Prefix))
                throw new InvalidOperationException("Enter an instance name prefix.");
            if (request.CounterDigits < 1)
                throw new InvalidOperationException("Counter digits must be at least 1.");
            if (request.StartNumber < 0)
                throw new InvalidOperationException("Start number cannot be negative.");
            if (request.InstanceCount < 1)
                throw new InvalidOperationException("Instance count must be at least 1.");
            if (request.FaceplateType.Width <= 0 || request.FaceplateType.Height <= 0)
                throw new InvalidOperationException("The template faceplate has invalid dimensions.");
            if (request.StartX < 0 || request.StartY < 0)
                throw new InvalidOperationException("Grid start coordinates cannot be negative.");
            if (request.HorizontalGap < 0 || request.VerticalGap < 0)
                throw new InvalidOperationException("Grid gaps cannot be negative.");

            try
            {
                checked
                {
                    int lastNumber = request.StartNumber + request.InstanceCount - 1;
                    _ = lastNumber.ToString("D" + request.CounterDigits);
                }
            }
            catch (OverflowException)
            {
                throw new InvalidOperationException("The start number and instance count exceed the supported range.");
            }
        }

        private static IList<string> BuildNames(BatchAddRequest request)
        {
            string prefix = request.Prefix.Trim();
            var names = new List<string>(request.InstanceCount);

            for (int index = 0; index < request.InstanceCount; index++)
            {
                int number = checked(request.StartNumber + index);
                names.Add(prefix + number.ToString("D" + request.CounterDigits));
            }

            if (names.Distinct(StringComparer.OrdinalIgnoreCase).Count() != names.Count)
            {
                throw new InvalidOperationException("Generated instance names are not unique.");
            }

            return names;
        }

        private static IList<GridPosition> BuildGridPositions(BatchAddRequest request)
        {
            int screenWidth = request.TargetScreen.Width;
            int screenHeight = request.TargetScreen.Height;
            int itemWidth = request.FaceplateType.Width;
            int itemHeight = request.FaceplateType.Height;

            if (request.StartX + itemWidth > screenWidth || request.StartY + itemHeight > screenHeight)
            {
                throw new InvalidOperationException("The first faceplate does not fit inside the target screen.");
            }

            int columnStep = checked(itemWidth + request.HorizontalGap);
            int rowStep = checked(itemHeight + request.VerticalGap);
            int columns = Math.Max(1, (screenWidth - request.StartX + request.HorizontalGap) / columnStep);
            var positions = new List<GridPosition>(request.InstanceCount);

            for (int index = 0; index < request.InstanceCount; index++)
            {
                int column = index % columns;
                int row = index / columns;
                int x = checked(request.StartX + column * columnStep);
                int y = checked(request.StartY + row * rowStep);

                if (x + itemWidth > screenWidth || y + itemHeight > screenHeight)
                {
                    throw new InvalidOperationException(
                        $"{request.InstanceCount} faceplates do not fit in the configured grid on screen '{request.TargetScreen.Name}'.");
                }

                positions.Add(new GridPosition(x, y));
            }

            return positions;
        }

        private static void ValidateNamesAreAvailable(HmiScreen targetScreen, IEnumerable<string> names)
        {
            var requestedNames = new HashSet<string>(names, StringComparer.OrdinalIgnoreCase);
            var conflicts = targetScreen.ScreenItems
                .Where(item => requestedNames.Contains(item.Name))
                .Select(item => item.Name)
                .ToList();

            if (conflicts.Count > 0)
            {
                throw new InvalidOperationException(
                    "The target screen already contains these object names: " + string.Join(", ", conflicts));
            }
        }

        private static string GetTemplateLabelValue(HmiFaceplateContainer templateContainer)
        {
            var label = templateContainer.Interface?.Find("Label");
            if (label == null)
            {
                throw new InvalidOperationException(
                    $"Faceplate type '{templateContainer.ContainedType}' on screen '{TemplateScreenName}' does not expose a 'Label' interface property.");
            }

            return label.Value?.ToString() ?? "";
        }

        private sealed class GridPosition
        {
            public GridPosition(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }
    }
}
