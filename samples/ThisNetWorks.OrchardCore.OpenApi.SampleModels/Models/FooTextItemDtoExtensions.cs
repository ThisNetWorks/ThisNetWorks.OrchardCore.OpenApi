namespace ThisNetWorks.OrchardCore.OpenApi.SampleModels.Models
{
    // This extends the FooTextItem to always have a contained part
    public partial class FooTextItemDto
    {
        public ContainedPartDto ContainedPart { get; set; }
    }
}
