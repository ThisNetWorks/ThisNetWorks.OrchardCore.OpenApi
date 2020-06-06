using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Linq;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class RemoveContentElementDocumentProcessor : IDocumentProcessor
    {
        public void Process(DocumentProcessorContext context)
        {
            context.TryRemoveDefinition("ContentElement");
            context.TryRemoveDefinition("ContentItem");
            context.TryRemoveDefinition("ContentField");
            context.TryRemoveDefinition("ContentPart");
        }
    }
}
