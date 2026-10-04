using Siemens.Engineering.HmiUnified;
using Siemens.Engineering.HmiUnified.UI.Screens;
using Siemens.Engineering.HmiUnified.UI.ScreenGroup;
using System.Collections.Generic;

namespace FKAroundTIA.Services
{
    public class ScreenService
    {
        public IList<HmiScreen> GetAllScreens(HmiSoftware hmiSoftware)
        {
            var screens = new List<HmiScreen>();
            if (hmiSoftware == null) return screens;

            AddScreens(hmiSoftware.Screens, screens);
            AddScreensFromGroups(hmiSoftware.ScreenGroups, screens);

            return screens;
        }

        public HmiScreen FindScreenRecursive(HmiSoftware hmiSoftware, string screenName)
        {
            if (hmiSoftware == null || string.IsNullOrEmpty(screenName)) return null;

            // Search in root screens
            var screen = hmiSoftware.Screens.Find(screenName);
            if (screen != null) return screen;

            // Search in screen groups recursively
            return FindScreenInGroups(hmiSoftware.ScreenGroups, screenName);
        }

        private HmiScreen FindScreenInGroups(HmiScreenGroupComposition groups, string screenName)
        {
            if (groups == null) return null;

            foreach (HmiScreenGroup group in groups)
            {
                var screen = group.Screens.Find(screenName);
                if (screen != null) return screen;

                var nestedScreen = FindScreenInGroups(group.Groups, screenName);
                if (nestedScreen != null) return nestedScreen;
            }

            return null;
        }

        private void AddScreens(HmiScreenComposition source, IList<HmiScreen> destination)
        {
            if (source == null) return;

            foreach (HmiScreen screen in source)
            {
                destination.Add(screen);
            }
        }

        private void AddScreensFromGroups(HmiScreenGroupComposition groups, IList<HmiScreen> destination)
        {
            if (groups == null) return;

            foreach (HmiScreenGroup group in groups)
            {
                AddScreens(group.Screens, destination);
                AddScreensFromGroups(group.Groups, destination);
            }
        }
    }
}
