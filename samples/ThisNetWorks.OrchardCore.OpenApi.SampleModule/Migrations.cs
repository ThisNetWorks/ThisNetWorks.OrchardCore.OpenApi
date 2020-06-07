using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Lists.Models;
using System.Threading.Tasks;
using OrchardCore.Recipes.Services;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly IRecipeMigrator _recipeMigrator;

        public Migrations(IContentDefinitionManager contentDefinitionManager,
            IRecipeMigrator recipeMigrator
            )
        {
            _contentDefinitionManager = contentDefinitionManager;
            _recipeMigrator = recipeMigrator;
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
            _contentDefinitionManager.AlterTypeDefinition("Bar", type => type
                .DisplayedAs("Bar")
                .WithPart("HtmlBodyPart")
            );

            _contentDefinitionManager.AlterTypeDefinition("Foo", type => type
                .DisplayedAs("Foo")
                .WithPart("MarkdownBodyPart")
            );

            _contentDefinitionManager.AlterTypeDefinition("Bag", type => type
                .DisplayedAs("Bag container")
                .Listable()
                .Creatable()
                .Securable()
                .Draftable()
                .Versionable()
                .WithPart("Items", "BagPart", part => part
                    .WithDescription("Item bag")
                    .WithSettings(new BagPartSettings
                    {
                        ContainedContentTypes = new string[] { "Bar", "Foo" }
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

        public int UpdateFrom3()
        {
            _contentDefinitionManager.AlterTypeDefinition("FooText", type => type
                .Securable()
                .Versionable()
                .DisplayedAs("Foo text")
                .WithPart("FooText")
            );

            _contentDefinitionManager.AlterPartDefinition("FooText", part => part
                .WithField("FooField", field => field
                    .OfType("TextField")
                    .WithDisplayName("Foo Field")
                )
            );

            return 4;
        }

        public int UpdateFrom4()
        {
            _contentDefinitionManager.AlterTypeDefinition("FooTextContainer", type => type
                .DisplayedAs("Foo text container")
                .Listable()
                .Securable()
                .Draftable()
                .Versionable()
                .WithPart("ListPart", part =>
                {
                    part.WithSettings(new ListPartSettings
                    {
                        ContainedContentTypes = new string[]
                        {
                            "FooText"
                        }
                    });
                })
            );

            return 5;
        }

        public async Task<int> UpdateFrom5Async()
        {
            // Creates some default content so the console app can use hard coded ids.
            await _recipeMigrator.ExecuteAsync("foo-text-container-content.json", this);

            return 6;
        }

        public async Task<int> UpdateFrom6Async()
        {
            // Gives edit and publish permission to the anonymous role
            // only for foo text content type.
            await _recipeMigrator.ExecuteAsync("anonymous-role.json", this);

            return 7;
        }

        public async Task<int> UpdateFrom7Async()
        {
            // Create a sql query with paramenters
            await _recipeMigrator.ExecuteAsync("sql-alias-query.json", this);

            return 8;
        }
    }
}