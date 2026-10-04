namespace FKAroundTIA.Models
{
    public class BatchAddRequest
    {
        public BatchAddFaceplateTypeInfo FaceplateType { get; set; }
        public BatchAddScreenInfo TargetScreen { get; set; }
        public string Prefix { get; set; }
        public int CounterDigits { get; set; }
        public int StartNumber { get; set; }
        public int InstanceCount { get; set; }
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int HorizontalGap { get; set; }
        public int VerticalGap { get; set; }
    }
}
