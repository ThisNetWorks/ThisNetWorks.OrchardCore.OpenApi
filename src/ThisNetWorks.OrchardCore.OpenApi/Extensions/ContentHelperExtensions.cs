using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThisNetWorks.OrchardCore.OpenApi.Extensions
{
    public static class ContentHelperExtensions
    {
        public static void TryRemoveDefinition(this DocumentProcessorContext context, string key)
        {
            if (context.Document.Definitions.TryGetValue(key, out var value))
            {
                value.AllOf.Clear();
                context.Document.Definitions.Remove(key);
            }
        }

        public static void ApplySchemaToContent(this IDictionary<string, OpenApiMediaType> content, JsonSchema schema)
        {
            var firstContentMediaType = content.FirstOrDefault();

            if (!String.IsNullOrEmpty(firstContentMediaType.Key))
            {
                content.Remove(firstContentMediaType.Key);
                content["application/json"] = new OpenApiMediaType
                {
                    Schema = new JsonSchema
                    {
                        Reference = schema
                    }
                };
            };
        }

        public static void AlterPathSchema(this DocumentProcessorContext context, string path, string operation, string response, JsonSchema contentItemDtoSchema, Action<OpenApiOperation> action = null)
        {
            if (context.Document.Paths.TryGetValue(path, out var pathValue))
            {
                if (pathValue.TryGetValue(operation, out var operationValue))
                {
                    if (operationValue.Responses.TryGetValue(response, out var responseValue))
                    {
                        responseValue.Content.ApplySchemaToContent(contentItemDtoSchema);
                    }

                    action?.Invoke(operationValue);
                }
            }
        }
    }
}
