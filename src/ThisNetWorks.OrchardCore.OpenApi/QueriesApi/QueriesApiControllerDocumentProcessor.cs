using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
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

                // Would be nice if this didn't have to be a dto. Or we could just ignore it?
                // Just ignoring it for now.
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
