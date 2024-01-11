using NSwag.AspNetCore;
using NSwag.Generation.AspNetCore;
using System;
using System.Collections.Generic;

namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiMiddlewareOptions
    {
        public List<Action<AspNetCoreOpenApiDocumentGeneratorSettings, IServiceProvider>> GeneratorOptions { get; set; } = new List<Action<AspNetCoreOpenApiDocumentGeneratorSettings, IServiceProvider>>();

        public bool UseOrchardCoreOpenApiDocumentMiddleware { get; set; } = true;

        public Action<OpenApiDocumentMiddlewareSettings> OpenApiDocumentMiddlewareSettings { get; set; } 

        public bool UseOrchardCoreSwaggerMiddleware { get; set; } = true;

        public Action<SwaggerUiSettings> SwaggerUISettings { get; set; }
    }
}
