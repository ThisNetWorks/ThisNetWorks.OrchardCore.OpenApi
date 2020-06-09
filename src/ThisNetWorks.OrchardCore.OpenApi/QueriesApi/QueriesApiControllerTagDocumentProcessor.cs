using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.QueriesApi
{
    public class QueriesApiControllerTagDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public QueriesApiControllerTagDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (string.IsNullOrEmpty(_openApiOptions.QueriesApi.ApiTag))
            {
                return;
            }

            // This just tweaks the api/content to a more useful path segment.
            context.Document.Paths.AlterApiControllerTag("/api/queries", _openApiOptions.QueriesApi.ApiTag);
        }
    }
}
