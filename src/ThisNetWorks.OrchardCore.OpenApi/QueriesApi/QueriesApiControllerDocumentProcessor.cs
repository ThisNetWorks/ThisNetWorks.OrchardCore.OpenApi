using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.QueriesApi
{
    public class QueriesApiControllerDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public QueriesApiControllerDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (!_openApiOptions.QueriesApi.AlterPathSchema)
            {
                return;
            }

            if (context.Document.Definitions.TryGetValue("ContentItemDto", out var contentItemDtoSchema))
            {
                var itemSchema = new JsonSchema
                {
                    Type = JsonObjectType.Array,
                    IsNullableRaw = true,
                    Item = new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        Reference = contentItemDtoSchema
                    }
                };

                // TODO This only works when returning content items.
                // Consider a different path for documents?
                context.Document.Definitions["QueriesItemsDto"] = itemSchema;

                context.AlterPathSchema("/api/queries/{name}", "post", "200", itemSchema, (operation) =>
                {
                    operation.OperationId = _openApiOptions.QueriesApi.PostOperationId;
                });
                context.AlterPathSchema("/api/queries/{name}", "get", "200", itemSchema, (operation) =>
                {
                    operation.OperationId = _openApiOptions.QueriesApi.GetOperationId;
                });
            }
        }
    }
}
