using Siemens.Engineering.HmiUnified.UI.Controls;

namespace FKAroundTIA.Models
{
    public class FaceplateInstanceInfo
    {
        public string Name { get; set; }
        public string FaceplateTypeName { get; set; }
        public string ParentGroup { get; set; }
        public string FullPath { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public HmiFaceplateContainer Container { get; set; }
    }
}
