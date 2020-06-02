using Microsoft.AspNetCore.Mvc.ModelBinding;
using OrchardCore.ContentManagement;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Settings;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.ViewModels
{
    public class SamplePartViewModel
    {
        public string MySetting { get; set; }

        public bool Show { get; set; }

        [BindNever]
        public ContentItem ContentItem { get; set; }

        [BindNever]
        public SamplePart SamplePart { get; set; }

        [BindNever]
        public SamplePartSettings Settings { get; set; }
    }
}
