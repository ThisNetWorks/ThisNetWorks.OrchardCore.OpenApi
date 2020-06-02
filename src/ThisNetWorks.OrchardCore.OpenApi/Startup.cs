using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using NSwag.Generation.Processors;
using OrchardCore.Modules;
using System;
using ThisNetWorks.OrchardCore.OpenApi.Options;
using ThisNetWorks.OrchardCore.OpenApi.Processors;

namespace ThisNetWorks.OrchardCore.OpenApi
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<OpenApiOptions>(o =>
            {
                o.IncludeAllFields = false;
                o.PathsToRemove.Add("api/lucene");
                o.PathsToRemove.Add("api/queries");
            });

            services.AddSingleton<IDocumentProcessor, RemoveControllersDocumentProcessor>();
            services.AddOpenApiDocument(config =>
            {
                config.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                };
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // TODo not using path yet (for tenants)
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var swaggerPath = httpContextAccessor.HttpContext.Request.PathBase + new PathString("/swagger");

            builder.UseOrchardCoreOpenApi(); // serve OpenAPI/Swagger documents with Orchard Core Content Types.

            //builder.UseOpenApi(); // Use OrchardCoreOpenApi to manage content types changing dynamically.
            builder.UseSwaggerUi3(); // serve Swagger UI
        }
    }
}