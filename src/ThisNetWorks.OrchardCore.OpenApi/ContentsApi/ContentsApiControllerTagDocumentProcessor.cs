using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Linq;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class ContentsApiControllerTagDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public ContentsApiControllerTagDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (string.IsNullOrEmpty(_openApiOptions.ContentsApi.ContentsApiTag))
            {
                return;
            }

            // This just tweaks the api/content to a more useful path segment.
            var contentApiPaths = context.Document.Paths.Where(x => x.Key.StartsWith("/api/content")).Select(x => x.Value);
            foreach (var pathItem in contentApiPaths)
            {
                foreach (var operation in pathItem)
                {
                    if (operation.Value.Tags.Any(x => string.Equals(x, "Api", StringComparison.OrdinalIgnoreCase)))
                    {
                        operation.Value.Tags.Clear();
                        operation.Value.Tags.Add(_openApiOptions.ContentsApi.ContentsApiTag);
                    }
                }
            }
        }
    }
}
