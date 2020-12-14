using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models
{
    public class SamplePart : ContentPart
    {
        public bool Show { get; set; }
        public TextField MyTextField { get; set; }
    }
}
