using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using NSwag.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.Middleware;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseOrchardCoreOpenApi(this IApplicationBuilder app, Action<OpenApiDocumentMiddlewareSettings> configure = null)
        {
            return UseOpenApiWithApiExplorerCore(app, configure);
        }

        private static IApplicationBuilder UseOpenApiWithApiExplorerCore(IApplicationBuilder app, Action<OpenApiDocumentMiddlewareSettings> configure)
        {
            var settings = configure == null ? app.ApplicationServices.GetService<IOptions<OpenApiDocumentMiddlewareSettings>>()?.Value : null ?? new OpenApiDocumentMiddlewareSettings();
            configure?.Invoke(settings);

            if (settings.Path.Contains("{documentName}"))
            {
                var documents = app.ApplicationServices.GetRequiredService<IEnumerable<OpenApiDocumentRegistration>>();
                foreach (var document in documents)
                {
                    app = app.UseMiddleware<OrchardCoreOpenApiDocumentMiddleware>(document.DocumentName, settings.Path.Replace("{documentName}", document.DocumentName), settings);
                }

                return app;
            }
            else
            {
                return app.UseMiddleware<OrchardCoreOpenApiDocumentMiddleware>(settings.DocumentName, settings.Path, settings);
            }
        }
    }
}
