using System.Linq;
using System.Threading.Tasks;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Settings;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.ViewModels;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Drivers
{
    public class SamplePartDisplayDriver : ContentPartDisplayDriver<SamplePart>
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public SamplePartDisplayDriver(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public override IDisplayResult Display(SamplePart SamplePart)
        {
            return Combine(
                Initialize<SamplePartViewModel>("SamplePart", async vm =>await BuildViewModelAsync(vm, SamplePart))
                    .Location("Detail", "Content:20"),
                Initialize<SamplePartViewModel>("SamplePart_Summary", async m =>await  BuildViewModelAsync(m, SamplePart))
                    .Location("Summary", "Meta:5")
            );
        }
        
        public override IDisplayResult Edit(SamplePart SamplePart)
        {
            return Initialize<SamplePartViewModel>("SamplePart_Edit", async vm => await BuildViewModelAsync(vm, SamplePart));
        }

        public override async Task<IDisplayResult> UpdateAsync(SamplePart model, IUpdateModel updater)
        {
            await updater.TryUpdateModelAsync(model, Prefix, t => t.Show);
            
            return Edit(model);
        }

        public async Task<SamplePartSettings> GetSamplePartSettingsAsync(SamplePart part)
        {
            var contentTypeDefinition = await _contentDefinitionManager.GetTypeDefinitionAsync(part.ContentItem.ContentType);
            var contentTypePartDefinition = contentTypeDefinition.Parts.FirstOrDefault(p => p.PartDefinition.Name == nameof(SamplePart));
            var settings = contentTypePartDefinition.GetSettings<SamplePartSettings>();

            return settings;
        }

        private async Task BuildViewModelAsync(SamplePartViewModel model, SamplePart part)
        {
            var settings = await GetSamplePartSettingsAsync(part);

            model.ContentItem = part.ContentItem;
            model.MySetting = settings.MySetting;
            model.Show = part.Show;
            model.SamplePart = part;
            model.Settings = settings;
        }
    }
}
