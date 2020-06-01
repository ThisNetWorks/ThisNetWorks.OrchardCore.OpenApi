using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NSwag.AspNetCore;
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
                //o.PathsToRemove.Add("content");
            });

            services.AddSingleton<IDocumentProcessor, ContentTypeSchemaProcessor>();
            services.AddSingleton<IDocumentProcessor, AlterExistingControllersSchemaProcessor>();
            services.AddSingleton<IDocumentProcessor, RestContentControllerSchemaProcessor>();
            services.AddOpenApiDocument(config =>
            {
                config.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver()
                };
                //config.DocumentProcessors.Add(new Processor());
                //config.SchemaProcessors.Add(new SchemaProcessor());
                //config.
            });

            // Somewhere in here we have to figure out how to clear this when content types change.
            services.Configure<OpenApiDocumentMiddlewareSettings>(o =>
            {

            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            // TODo not using path yet (for tenants)
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var swaggerPath = httpContextAccessor.HttpContext.Request.PathBase + new PathString("/swagger");

            builder.UseOrchardCoreOpenApi(o =>
            {
                //o.
            }); // serve OpenAPI/Swagger documents

            //builder.UseOpenApi(); // Use OrchardCoreOpenApi to manage content types changing dynamically.
            builder.UseSwaggerUi3(); // serve Swagger UI
        }
    }
}