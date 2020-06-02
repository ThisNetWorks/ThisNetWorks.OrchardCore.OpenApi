using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class RemoveContentElementDocumentProcessor : IDocumentProcessor
    {
        public void Process(DocumentProcessorContext context)
        {
            // TODO ContentItem and ContentElement are cross referenced somewhere still.
            // So cannot be removed (yet).
            //context.TryRemoveDefinition("ContentElement");
            context.TryRemoveDefinition("ContentField");
            context.TryRemoveDefinition("ContentPart");

            if (context.Document.Definitions.TryGetValue("ContentItem", out var contentItem))
            {
                contentItem.AllOf.Clear();
                contentItem.Reference = null;
            }

        }
    }
}
