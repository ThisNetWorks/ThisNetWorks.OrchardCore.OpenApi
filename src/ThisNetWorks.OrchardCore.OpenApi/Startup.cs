using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            });

            services.AddSingleton<IDocumentProcessor, ContentTypeSchemaProcessor>();
            services.AddOpenApiDocument(config =>
            {
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

            builder.UseOpenApi(); // serve OpenAPI/Swagger documents
            builder.UseSwaggerUi3(); // serve Swagger UI
        }
    }
}