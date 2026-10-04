using System.Collections.Generic;

namespace FKAroundTIA.Models
{
    public class FaceplateTypeGroupInfo
    {
        public string TypeName { get; set; }
        public int Count { get; set; }
        public IList<FaceplateInstanceInfo> Instances { get; set; }
        public IList<FaceplateInterfaceItemInfo> InterfaceProperties { get; set; }
        public System.Data.DataTable GridData { get; set; }
    }
}
