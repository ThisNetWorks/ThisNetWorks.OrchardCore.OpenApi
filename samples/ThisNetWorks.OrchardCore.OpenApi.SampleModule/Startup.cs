using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.GraphQL.Options;
using OrchardCore.ContentTypes.Editors;
using OrchardCore.Data.Migration;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Html.Models;
using OrchardCore.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using ThisNetWorks.OrchardCore.OpenApi.Options;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Drivers;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Handlers;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Settings;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule
{
    public class Startup : StartupBase
    {
        private readonly IShellConfiguration _shellConfiguration;

        public Startup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var swaggerUiClientSecret = _shellConfiguration.GetValue<string>("SwaggerUiClientSecret");

            services.AddContentPart<SamplePart>()
                .UseDisplayDriver<SamplePartDisplayDriver>()
                .AddHandler<SamplePartHandler>();

            services.AddScoped<IContentPartDefinitionDisplayDriver, SamplePartSettingsDisplayDriver>();
            services.AddScoped<IDataMigration, Migrations>();

            services.Configure<OpenApiOptions>(o =>
            {
                // This only includes fields that are used in parts.
                o.ContentTypes.IncludeAllFields = false;

                // This is a sample of how to use OpenID Connect with OpenAPI.
                o.Middleware.GeneratorOptions.Add((generator, serviceProvider) =>
                {
                    generator.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Description = "OpenID Connect",
                        Flow = OpenApiOAuth2Flow.AccessCode,
                        Flows = new OpenApiOAuthFlows()
                        {
                            AuthorizationCode = new OpenApiOAuthFlow()
                            {
                                Scopes = new Dictionary<string, string>
                                {
                                { "openid", "OpenID" },
                                { "profile", "Profile" },
                                { "roles", "Roles" }
                                },
                                AuthorizationUrl = "/connect/authorize",
                                TokenUrl = "/connect/token"
                            },
                        }
                    });
                    generator.OperationProcessors.Add(
                        new AspNetCoreOperationSecurityScopeProcessor("bearer"));
                });
                o.Middleware.SwaggerUi3Settings = (swagger) =>
                {
                    swagger.OAuth2Client = new OAuth2ClientSettings
                    {
                        ClientId = "openapi_auth_code_flow",
                        ClientSecret = swaggerUiClientSecret
                    };
                };
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