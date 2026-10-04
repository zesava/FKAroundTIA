using System.Collections.Generic;
using System.Linq;

namespace FKAroundTIA.Models
{
    public class ScreenFaceplateInfo
    {
        public string Name { get; set; }
        public IList<FaceplateTypeGroupInfo> Groups { get; set; }

        public int InstanceCount => Groups?.Sum(group => group.Count) ?? 0;
    }
}
