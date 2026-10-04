using Siemens.Engineering.HmiUnified.UI.Screens;

namespace FKAroundTIA.Models
{
    public class BatchAddScreenInfo
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public HmiScreen Screen { get; set; }

        public override string ToString()
        {
            return Name ?? "";
        }
    }
}
