using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Settings
{
    public class SamplePartSettingsViewModel
    {
        public string MySetting { get; set; }

        [BindNever]
        public SamplePartSettings SamplePartSettings { get; set; }
    }
}
