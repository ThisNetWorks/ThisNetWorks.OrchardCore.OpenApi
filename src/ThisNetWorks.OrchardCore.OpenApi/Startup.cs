using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NJsonSchema;
using NJsonSchema.NewtonsoftJson.Generation;
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
            services.AddOptions<OpenApiOptions>();
            services.Configure<OpenApiOptions>(o =>
            {
                o.Middleware.GeneratorOptions.Add((o, sp) =>
                {
                    o.SchemaSettings = new NewtonsoftJsonSchemaGeneratorSettings
                    {
                        SerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new DefaultContractResolver()
                        },
                        SchemaType = SchemaType.OpenApi3
                    };
                });
            });

            services.AddSingleton<IDocumentProcessor, RemoveControllersDocumentProcessor>();

            services.AddOpenApiDocument((o, sp) =>
            {
                var options = sp.GetRequiredService<IOptions<OpenApiOptions>>().Value;
                foreach (var option in options.Middleware.GeneratorOptions)
                {
                    option.Invoke(o, sp);
                }
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<OpenApiOptions>>().Value;

            if (options.Middleware.UseOrchardCoreOpenApiDocumentMiddleware)
            {
                // serve OpenAPI/Swagger documents with Orchard Core Content Types.
                builder.UseOrchardCoreOpenApi(options.Middleware.OpenApiDocumentMiddlewareSettings);
            }

            if (options.Middleware.UseOrchardCoreSwaggerMiddleware)
            {
                builder.UseSwaggerUi(options.Middleware.SwaggerUISettings);
            }
        }
    }
}