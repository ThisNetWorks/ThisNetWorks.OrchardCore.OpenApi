using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.LuceneApi
{
    public class LuceneApiControllerTagDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public LuceneApiControllerTagDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (string.IsNullOrEmpty(_openApiOptions.LuceneApi.ApiTag))
            {
                return;
            }

            // This just tweaks the api/content to a more useful path segment.
            context.Document.Paths.AlterApiControllerTag("/api/lucene", _openApiOptions.LuceneApi.ApiTag);
        }
    }
}
