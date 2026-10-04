using Siemens.Engineering;
using Siemens.Engineering.HW.Features;
using Siemens.Engineering.HmiUnified;
using FKAroundTIA.Models;
using System.Collections.Generic;
using Siemens.Engineering.HW;

namespace FKAroundTIA.Services
{
    public class RuntimeService
    {
        public IList<RuntimeInfo> GetRuntimes(Project project)
        {
            var runtimes = new List<RuntimeInfo>();
            if (project == null) return runtimes;

            foreach (Device device in project.Devices)
            {
                FindRuntimesRecursive(device.DeviceItems, device.Name, runtimes);
            }
            return runtimes;
        }

        private void FindRuntimesRecursive(DeviceItemComposition items, string deviceName, List<RuntimeInfo> runtimes)
        {
            if (items == null) return;

            foreach (DeviceItem item in items)
            {
                var softwareContainer = item.GetService<SoftwareContainer>();
                if (softwareContainer != null && softwareContainer.Software is HmiSoftware)
                {
                    runtimes.Add(new RuntimeInfo
                    {
                        Name = item.Name,
                        DeviceName = deviceName,
                        DeviceItem = item
                    });
                }

                FindRuntimesRecursive(item.DeviceItems, deviceName, runtimes);
            }
        }
    }
}
