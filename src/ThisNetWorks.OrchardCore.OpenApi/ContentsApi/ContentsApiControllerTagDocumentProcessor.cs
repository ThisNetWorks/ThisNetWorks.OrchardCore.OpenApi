using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Linq;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class ContentsApiControllerTagDocumentProcessor : IDocumentProcessor
    {
        public void Process(DocumentProcessorContext context)
        {
            // This just tweaks the api/content to a more useful path segment.
            var contentApiPaths = context.Document.Paths.Where(x => x.Key.StartsWith("/api/content")).Select(x => x.Value);
            foreach (var pathItem in contentApiPaths)
            {
                foreach (var operation in pathItem)
                {
                    if (operation.Value.Tags.Any(x => string.Equals(x, "Api", StringComparison.OrdinalIgnoreCase)))
                    {
                        operation.Value.Tags.Clear();
                        operation.Value.Tags.Add("Content");
                    }
                }
            }
        }
    }
}
