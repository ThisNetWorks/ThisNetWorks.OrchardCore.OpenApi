using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule
{
    public class Migrations : DataMigration
    {
        IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            _contentDefinitionManager.AlterPartDefinition("SamplePart", builder => builder
                .Attachable()
                .WithDescription("Provides a Sample part for your content item."));

            return 1;
        }

        // This creates some useful type definitions for bag parts.
        public int UpdateFrom1()
        {
            _contentDefinitionManager.AlterTypeDefinition("SampleBar", type => type
                .DisplayedAs("Sample bar")
                .WithPart("HtmlBodyPart")
            );

            _contentDefinitionManager.AlterTypeDefinition("SampleFoo", type => type
                .DisplayedAs("Sample foo")
                .WithPart("MarkdownBodyPart")
            );

            _contentDefinitionManager.AlterTypeDefinition("SampleBag", type => type
                .DisplayedAs("Sample bag container")
                .Listable()
                .Creatable()
                .Securable()
                .Draftable()
                .Versionable()
                .WithPart("Samples", "BagPart", part => part
                    .WithDescription("Sample bag")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new string[] { "SampleBar", "SampleFoo" }
                    }
                )
            ));

            return 2;
        }

        // This adds a field to the HtmlBodyPart
        public int UpdateFrom2()
        {
            _contentDefinitionManager.AlterPartDefinition("HtmlBodyPart", part => part
                .WithField("FooField", field => field
                    .OfType("TextField")
                    .WithDisplayName("Foo Field")
                )
            );

            return 3;
        }
    }
}