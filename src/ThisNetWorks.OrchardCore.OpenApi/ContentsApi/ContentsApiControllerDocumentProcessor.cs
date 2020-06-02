using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class ContentsApiControllerDocumentProcessor : IDocumentProcessor
    {
        public void Process(DocumentProcessorContext context)
        {
            if (context.Document.Definitions.TryGetValue("ContentItemDto", out var contentItemDtoSchema))
            {
                context.AlterPathSchema("/api/content/{contentItemId}", "get", "200", contentItemDtoSchema);

                context.AlterPathSchema("/api/content/{contentItemId}", "delete", "200", contentItemDtoSchema);

                context.AlterPathSchema("/api/content", "post", "200", contentItemDtoSchema,
                    (operation) => operation.RequestBody.Content.ApplySchemaToContent(contentItemDtoSchema));
            }
        }
    }
}
