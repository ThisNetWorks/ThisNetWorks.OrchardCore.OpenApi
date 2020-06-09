using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class RemoveContentElementDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public RemoveContentElementDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (!_openApiOptions.ContentsApi.RemoveContentElements)
            {
                return;
            }

            context.TryRemoveDefinition("ContentElement");
            context.TryRemoveDefinition("ContentItem");
            context.TryRemoveDefinition("ContentField");
            context.TryRemoveDefinition("ContentPart");
        }
    }
}
