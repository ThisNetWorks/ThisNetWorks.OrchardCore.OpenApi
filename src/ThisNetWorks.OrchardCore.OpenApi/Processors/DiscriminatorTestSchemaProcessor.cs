using NJsonSchema;
using NJsonSchema.Generation;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.Models;

namespace ThisNetWorks.OrchardCore.OpenApi.Processors
{
    public class DiscriminatorTestSchemaProcessor : IDocumentProcessor
    {

        public void Process(DocumentProcessorContext context)
        {
            var childSchema = new JsonSchema
            {
                Type = JsonObjectType.Object,
            };



            var typeDtoSchema = context.SchemaGenerator.Generate(typeof(ContentItemDto), context.SchemaResolver);
            typeDtoSchema.AllowAdditionalProperties = false;

            typeDtoSchema.ActualTypeSchema.Definitions["Foo"] = childSchema;

            typeDtoSchema.ActualTypeSchema.DiscriminatorObject = new OpenApiDiscriminator
            {
                PropertyName = "discr",
                Mapping =
                {
                    {
                        "Bar",
                        new JsonSchema
                        {
                            Reference = childSchema
                        }
                    }
                }
            };

            var json = typeDtoSchema.ActualTypeSchema.ToJson();

            //var json = context.Document.Definitions["ContentItemDto"].ToJson();

            //var schema = new JsonSchema();
            //schema.Definitions["Foo"] = childSchema;
            //schema.DiscriminatorObject = new OpenApiDiscriminator
            //{
            //    PropertyName = "discr",
            //    Mapping =
            //    {
            //        {
            //            "Bar",
            //            new JsonSchema
            //            {
            //                Reference = childSchema
            //            }
            //        }
            //    }
            //};

            //var json = schema.ToJson();
        }
    }
}
