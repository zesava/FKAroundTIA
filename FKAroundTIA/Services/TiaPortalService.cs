using Siemens.Engineering;
using System;
using System.Linq;

namespace FKAroundTIA.Services
{
    public class TiaPortalService
    {
        public TiaPortal AttachedPortal { get; private set; }
        public Project ActiveProject { get; private set; }

        public Project Connect()
        {
            var processes = TiaPortal.GetProcesses();
            if (processes == null || processes.Count == 0)
            {
                throw new InvalidOperationException("TIA Portal is not running. Please start TIA Portal first.");
            }

            // Attach to the first running process
            var process = processes.First();
            AttachedPortal = process.Attach();

            if (AttachedPortal.Projects == null || AttachedPortal.Projects.Count == 0)
            {
                throw new InvalidOperationException("No open project found in TIA Portal. Please open a project first.");
            }

            ActiveProject = AttachedPortal.Projects.First();
            return ActiveProject;
        }
    }
}
