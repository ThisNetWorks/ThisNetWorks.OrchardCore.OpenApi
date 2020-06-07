using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.GraphQL.Options;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Html.Models;
using OrchardCore.Modules;
using System;
using ThisNetWorks.OrchardCore.OpenApi.Options;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Drivers;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Handlers;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Settings;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddContentPart<SamplePart>()
                .UseDisplayDriver<SamplePartDisplayDriver>()
                .AddHandler<SamplePartHandler>();

            services.AddScoped<IContentPartDefinitionDisplayDriver, SamplePartSettingsDisplayDriver>();
            services.AddScoped<IDataMigration, Migrations>();

            services.Configure<OpenApiOptions>(o =>
            {
                // This only includes fields that are used in parts.
                o.ContentTypes.IncludeAllFields = false;
            });

            services.Configure<GraphQLContentOptions>(o =>
            {
                // Because there is an extra field added to this part, and an issue with duplicate fields
                // TODO remove when issue resolved.
                o.ConfigurePart<HtmlBodyPart>(part =>
                {
                    part.Hidden = true;
                });
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaControllerRoute(
                name: "Home",
                areaName: "ThisNetWorks.OrchardCore.OpenApi.SampleModule",
                pattern: "Home/Index",
                defaults: new { controller = "Home", action = "Index" }
            );
        }
    }
}