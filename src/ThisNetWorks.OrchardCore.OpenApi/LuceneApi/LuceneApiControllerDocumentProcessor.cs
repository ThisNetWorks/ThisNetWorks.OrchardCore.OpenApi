using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.LuceneApi
{
    public class LuceneApiControllerDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public LuceneApiControllerDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (!_openApiOptions.LuceneApi.AlterPathSchema)
            {
                return;
            }

            if (context.Document.Definitions.TryGetValue("ContentItemDto", out var contentItemDtoSchema))
            {
                var propertySchema = new JsonSchemaProperty
                {
                    Type = JsonObjectType.Array,
                    IsNullableRaw = true,
                    Item = new JsonSchemaProperty
                    {
                        Type = JsonObjectType.Object,
                        IsNullableRaw = true,
                        Reference = contentItemDtoSchema
                    }
                };

                var itemSchema = new JsonSchema
                {
                    Type = JsonObjectType.Object,
                    IsNullableRaw = true
                };

                itemSchema.Properties["Items"] = propertySchema;

                // Would be nice if this didn't have to be a dto. Or we could just ignore it?
                // Just ignoring it for now.
                context.Document.Definitions["LuceneItemsDto"] = itemSchema;

                context.AlterPathSchema("/api/lucene/content", "post", "200", itemSchema, (operation) =>
                {
                    operation.OperationId = _openApiOptions.LuceneApi.ContentPostOperationId;
                });
                context.AlterPathSchema("/api/lucene/content", "get", "200", itemSchema, (operation) =>
                {
                    operation.OperationId = _openApiOptions.LuceneApi.ContentGetOperationId;
                });

                // The document schema can be anything, so everything is stored in additional properties as kvps.
                var referenceSchema = new JsonSchema
                {
                    Type = JsonObjectType.Object,
                    AllowAdditionalProperties = true
                };

                var documentsSchema = new JsonSchema
                {
                    Type = JsonObjectType.Array,
                    IsNullableRaw = true,
                    Item = new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        Reference = referenceSchema
                    }
                };

                context.Document.Definitions["LuceneDocumentDto"] = referenceSchema;

                context.Document.Definitions["LuceneDocumentsDto"] = documentsSchema;

                context.AlterPathSchema("/api/lucene/documents", "post", "200", documentsSchema, (operation) =>
                {
                    operation.OperationId = _openApiOptions.LuceneApi.DocumentsPostOperationId;
                });
                context.AlterPathSchema("/api/lucene/documents", "get", "200", documentsSchema, (operation) =>
                {
                    operation.OperationId = _openApiOptions.LuceneApi.DocumentsGetOperationId;
                });
            }
        }
    }
}
