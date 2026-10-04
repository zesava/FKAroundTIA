using Siemens.Engineering.HmiUnified.UI.Controls;

namespace FKAroundTIA.Models
{
    public class BatchAddFaceplateTypeInfo
    {
        public string TypeName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public HmiFaceplateContainer TemplateContainer { get; set; }

        public override string ToString()
        {
            return TypeName ?? "";
        }
    }
}
