using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Drivers;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Handlers;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Settings;
using OrchardCore.Modules;

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