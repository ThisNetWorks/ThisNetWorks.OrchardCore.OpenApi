using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.Processors
{
    public class AlterExistingControllersSchemaProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;
        private readonly ContentOptions _contentOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private IContentDefinitionManager _contentDefinitionManager;

        public AlterExistingControllersSchemaProcessor(
            IOptions<OpenApiOptions> openApiOptions,
            IOptions<ContentOptions> contentOptions,
            IHttpContextAccessor httpContentAccessor
            )
        {
            _openApiOptions = openApiOptions.Value;
            _httpContextAccessor = httpContentAccessor;
            _contentOptions = contentOptions.Value;
        }
        public void Process(DocumentProcessorContext context)
        {
            foreach(var pathToRemove in _openApiOptions.PathsToRemove)
            {

                var paths = context.Document.Paths.Where(x => x.Key.Contains(pathToRemove));
                foreach (var path in paths.Select(x => x.Key))
                {
                    context.Document.Paths.Remove(path);
                }
            }

            // This just tweaks the api/content to a more useful path segment.
            // Probably doable from nswag.config as well. Generation Mode.
            var pathTags = context.Document.Paths.Where(x => x.Key.Contains("api/content")).Select(x => x.Value);
            foreach(OpenApiPathItem pathItem in pathTags)
            {
                foreach(var pa in pathItem)
                {
                    if (pa.Value.Tags.Any(x => string.Equals(x, "Api", StringComparison.OrdinalIgnoreCase)))
                    {
                        pa.Value.Tags.Insert(0, "Content");
                    }
                }
            }
        }
    }
}
