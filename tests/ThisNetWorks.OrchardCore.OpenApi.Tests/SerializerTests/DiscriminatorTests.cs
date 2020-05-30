using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.SerializerTests
{
    public class DiscriminatorTests
    {
        [Fact]
        public async Task When_discriminator_object_is_set_then_schema_is_correctly_serialized()
        {
            //// Arrange
            var childSchema = new JsonSchema
            {
                Type = JsonObjectType.Object,
            };

            var schema = new JsonSchema();
            schema.Definitions["Foo"] = childSchema;
            schema.DiscriminatorObject = new OpenApiDiscriminator
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

            //// Act
            var json = schema.ToJson();
            var schema2 = await JsonSchema.FromJsonAsync(json);
            var json2 = schema2.ToJson();

            //// Assert
            Assert.Contains(@"""propertyName"": ""discr""", json);
            Assert.Contains(@"""Bar"": ""#/definitions/Foo""", json);

            Assert.Equal(json, json2);

            Assert.Equal(schema2.Definitions["Foo"], schema2.ActualDiscriminatorObject.Mapping["Bar"].ActualSchema);
        }
    }
}
