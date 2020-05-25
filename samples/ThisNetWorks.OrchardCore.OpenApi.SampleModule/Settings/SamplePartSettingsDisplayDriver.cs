using System;
using System.Threading.Tasks;
using OrchardCore.ContentManagement.Metadata.Models;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.DisplayManagement.Views;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Settings
{
    public class SamplePartSettingsDisplayDriver : ContentPartDefinitionDisplayDriver
    {
        public override IDisplayResult Edit(ContentPartDefinition contentPartDefinition)
        {
            if (!String.Equals(nameof(SamplePart), contentPartDefinition.Name))
            {
                return null;
            }

            return Initialize<SamplePartSettingsViewModel>("SamplePartSettings_Edit", model =>
            {
                var settings = contentPartDefinition.GetSettings<SamplePartSettings>();

                model.MySetting = settings.MySetting;
                model.SamplePartSettings = settings;
            }).Location("Content");
        }

        public override async Task<IDisplayResult> UpdateAsync(ContentPartDefinition contentPartDefinition, UpdatePartEditorContext context)
        {
            if (!String.Equals(nameof(SamplePart), contentPartDefinition.Name))
            {
                return null;
            }

            var model = new SamplePartSettingsViewModel();

            if (await context.Updater.TryUpdateModelAsync(model, Prefix, m => m.MySetting))
            {
                context.Builder.WithSettings(new SamplePartSettings { MySetting = model.MySetting });
            }

            return Edit(contentPartDefinition);
        }
    }
}
