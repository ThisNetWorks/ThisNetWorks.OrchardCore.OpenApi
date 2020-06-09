using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class ContentsApiControllerDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public ContentsApiControllerDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (!_openApiOptions.ContentsApi.AlterPathSchema)
            {
                return;
            }

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
