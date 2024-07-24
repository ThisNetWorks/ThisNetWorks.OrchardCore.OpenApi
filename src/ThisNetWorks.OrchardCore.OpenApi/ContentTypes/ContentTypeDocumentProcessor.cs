using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NJsonSchema.NewtonsoftJson.Converters;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Mvc.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using ThisNetWorks.OrchardCore.OpenApi.Models;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentTypes
{
    public class ContentTypeDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;
        private readonly ContentOptions _contentOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IContentDefinitionManager _contentDefinitionManager;

        public ContentTypeDocumentProcessor(
            IOptions<OpenApiOptions> openApiOptions,
            IOptions<ContentOptions> contentOptions,
            IHttpContextAccessor httpContentAccessor
            )
        {
            _openApiOptions = openApiOptions.Value;
            _httpContextAccessor = httpContentAccessor;
            _contentOptions = contentOptions.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (!_openApiOptions.ContentTypes.ProcessContentTypes)
            {
                return;
            }

            _contentDefinitionManager ??= _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IContentDefinitionManager>();

            var contentElementDtoSchema = context.SchemaGenerator.Generate(typeof(ContentElementDto), context.SchemaResolver);
            // Due to issue with NJsonSchema we force set additional properties here to false
            // because it is defined on the base class.
            // cf. https://github.com/RicoSuter/NSwag/issues/2818
            contentElementDtoSchema.AllowAdditionalProperties = false;
            var fieldDtoSchema = context.SchemaGenerator.Generate(typeof(ContentFieldDto), context.SchemaResolver);
            fieldDtoSchema.AllowAdditionalProperties = false;

            var ctds = _contentDefinitionManager.ListTypeDefinitionsAsync().GetAwaiter().GetResult();
            var allFieldDefinitions = ctds
                .SelectMany(x => x.Parts.SelectMany(p => p.PartDefinition.Fields))
                .Select(x => x.FieldDefinition).ToLookup(x => x.Name, StringComparer.OrdinalIgnoreCase);

            // Process fields
            foreach (var contentFieldOption in _contentOptions.ContentFieldOptions)
            {
                if (!_openApiOptions.ContentTypes.IncludeAllFields)
                {
                    if (!allFieldDefinitions.Contains(contentFieldOption.Type.Name))
                    {
                        continue;
                    }
                }

                if (_openApiOptions.ContentTypes.ExcludedFields.Any(x => string.Equals(x, contentFieldOption.Type.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var fieldSchema = context.SchemaGenerator.Generate(contentFieldOption.Type, context.SchemaResolver);
                fieldSchema.AllOf.ElementAt(1).AllowAdditionalProperties = false;
                // remove first AllOf and reinsert the fieldDtoSchema as the ref.
                InsertDtoReferenceSchema(fieldSchema, fieldDtoSchema);

                // Change schema registration name to 'TextFieldDto'               
                AlterSchemaDefinition(context, contentFieldOption.Type.Name, _openApiOptions.ContentTypes.SchemaNameExtension);
            }

            // Process Parts
            // Note we have to use the content definition manager here
            // because parts may be created dynamically.
            // We also include definitions for code only parts, like ContainedPart
            var contentItemSchema = context.SchemaResolver.GetSchema(typeof(ContentItem), false);
            // This should end up being ignored, because we remove it from the nswag.config
            var contentItemDtoSchema = context.SchemaGenerator.Generate(typeof(ContentItemDto), context.SchemaResolver);
            contentItemDtoSchema.AllowAdditionalProperties = false;
            //var contentItemDtoSchema = context.Document.Definitions["ContentItemDto"];

            var partDtoSchema = context.SchemaGenerator.Generate(typeof(ContentPartDto), context.SchemaResolver);

            partDtoSchema.AllowAdditionalProperties = false;
            var allPartDefinitions = _contentDefinitionManager.ListPartDefinitionsAsync().GetAwaiter().GetResult();

            // Register code only parts first.
            foreach (var registeredPartOption in _contentOptions.ContentPartOptions)
            {
                // Is also registered in the part definitions.
                if (allPartDefinitions.Any(x => x.Name == registeredPartOption.Type.Name))
                {
                    continue;
                }
                // Has no fields as it is code only
                var partSchema = context.SchemaGenerator.Generate(registeredPartOption.Type, context.SchemaResolver);

                partSchema.AllOf.ElementAt(1).AllowAdditionalProperties = false;
                // remove first AllOf and reinsert the partDtoSchema as the ref.
                InsertDtoReferenceSchema(partSchema, partDtoSchema);

                // Use ActualProperties here, not Properties
                AlterArrayPropertiesToContentItemDtoSchema(partSchema.ActualProperties, contentItemSchema, contentItemDtoSchema);
                // Change schema regisitration name to 'ContainedPartDto'
                AlterSchemaDefinition(context, registeredPartOption.Type.Name, _openApiOptions.ContentTypes.SchemaNameExtension);
            }

            // Then register parts defined in the part definitions.
            foreach (var partDefinition in allPartDefinitions)
            {
                // check to see if it is a hard typed part.
                if (_contentOptions.ContentPartOptionsLookup.TryGetValue(partDefinition.Name, out var contentPartOption) &&
                    !_openApiOptions.ContentTypes.TreatPartsAsDynamic.Any(x => x == partDefinition.Name))
                {
                    var partSchema = context.SchemaGenerator.Generate(contentPartOption.Type, context.SchemaResolver);
 
                    // When a ContentField is defined on the Model, we need to remove it from the AllOf schema
                    // It will be included by the part definition.
                    foreach(var allOf in partSchema.AllOf)
                    {
                        var propertiesToRemove = new List<string>();
                        foreach(var property in allOf.Properties)
                        {
                            if (property.Value.OneOf.Any())
                            {
                                propertiesToRemove.Add(property.Key);

                            }
                        }
                        foreach(var property in propertiesToRemove)
                        {
                            allOf.Properties.Remove(property);
                        }
                    }

                    partSchema.AllOf.ElementAt(1).AllowAdditionalProperties = false;
                    // remove first AllOf and reinsert the partDtoSchema as the ref.
                    InsertDtoReferenceSchema(partSchema, partDtoSchema);

                    //TODO test this, it's being added as a direct property
                    // but the other properties are added as part of all of
                    // so we might need to find all of index 1 and insert it there
                    foreach (var field in partDefinition.Fields)
                    {
                        // Lookup field definition.
                        if (context.Document.Definitions.TryGetValue(field.FieldDefinition.Name + _openApiOptions.ContentTypes.SchemaNameExtension, out var fieldSchema))
                        {
                            // Add field as property.
                            var propertySchema = new JsonSchemaProperty
                            {
                                Type = JsonObjectType.Object,
                                IsNullableRaw = true
                            };

                            propertySchema.OneOf.Add(new JsonSchema
                            {
                                Type = JsonObjectType.Object,
                                Reference = fieldSchema.ActualSchema
                            });
                            partSchema.Properties[field.Name] = propertySchema;
                        }
                    }

                    foreach (var allOfSchema in partSchema.AllOf)
                    {
                        AlterArrayPropertiesToContentItemDtoSchema(allOfSchema.ActualProperties, contentItemSchema, contentItemDtoSchema);
                    }
                    // Change schema regisitration name to 'HtmlPartDto'
                    AlterSchemaDefinition(context, contentPartOption.Type.Name, _openApiOptions.ContentTypes.SchemaNameExtension);

                }
                else // This builds dynamic parts.
                {
                    // We need to skip registrations for type definition parts.
                    if (ctds.Any(x => x.Name == partDefinition.Name))
                    {
                        continue;
                    }
                    var partReferenceSchema = new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        Reference = partDtoSchema.ActualSchema,
                        AllowAdditionalProperties = false
                    };
                    var partSchema = new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        AllowAdditionalProperties = false
                    };

                    partSchema.AllOf.Add(partReferenceSchema);
                    foreach (var field in partDefinition.Fields)
                    {
                        // Lookup field definition.
                        if (context.Document.Definitions.TryGetValue(field.FieldDefinition.Name + _openApiOptions.ContentTypes.SchemaNameExtension, out var fieldSchema))
                        {
                            // Add field as property.
                            var propertySchema = new JsonSchemaProperty
                            {
                                Type = JsonObjectType.Object,
                                IsNullableRaw = true
                            };

                            propertySchema.OneOf.Add(new JsonSchema
                            {
                                Type = JsonObjectType.Object,
                                Reference = fieldSchema.ActualSchema
                            });
                            partSchema.Properties[field.Name] = propertySchema;
                        }
                    }

                    context.Document.Definitions[partDefinition.Name + _openApiOptions.ContentTypes.SchemaNameExtension] = partSchema;
                }
            }

            // Content Types
            var typeDtoSchema = context.SchemaGenerator.Generate(typeof(ContentItemDto), context.SchemaResolver);
            typeDtoSchema.AllowAdditionalProperties = false;
            typeDtoSchema.ActualTypeSchema.DiscriminatorObject = new OpenApiDiscriminator
            {
                //PropertyName = "discriminator",
                PropertyName = "ContentType",
                // TODO a custom one of these might help create types automatically.
                // Particularly useful for Flow.ContentItems etc.
                JsonInheritanceConverter = new JsonInheritanceConverter("ContentType")
            };

            foreach (var ctd in ctds)
            {
                if (_openApiOptions.ContentTypes.ExcludedTypes.Any(x => string.Equals(x, ctd.Name)))
                {
                    continue;
                }
                var typeReferenceSchema = new JsonSchema
                {
                    Type = JsonObjectType.Object,
                    Reference = typeDtoSchema.ActualSchema,
                    AllowAdditionalProperties = false
                };

                var typeSchema = new JsonSchema
                {
                    Type = JsonObjectType.Object,
                    AllowAdditionalProperties = false
                };

                typeSchema.AllOf.Add(typeReferenceSchema);
                var typeFieldPartDefinition = ctd.Parts.FirstOrDefault(x => x.PartDefinition.Name == ctd.Name);
                // Not all content types will have a 'FieldDefinitionPart' until fields are added.
                if (typeFieldPartDefinition != null)
                {
                    // Add Field Part container
                    var partSchema = new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        AllowAdditionalProperties = false
                    };
                    var partReferenceSchema = new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        AllowAdditionalProperties = false,
                        Reference = partDtoSchema.ActualSchema
                    };
                    partSchema.AllOf.Add(partReferenceSchema);
                    // TODO move to method
                    foreach (var field in typeFieldPartDefinition.PartDefinition.Fields)
                    {
                        // Lookup field definition.
                        if (context.Document.Definitions.TryGetValue(field.FieldDefinition.Name + _openApiOptions.ContentTypes.SchemaNameExtension, out var fieldSchema))
                        {
                            // Add field as property.
                            var propertySchema = new JsonSchemaProperty
                            {
                                Type = JsonObjectType.Object,
                                IsNullableRaw = true
                            };

                            propertySchema.OneOf.Add(new JsonSchema
                            {
                                Type = JsonObjectType.Object,
                                Reference = fieldSchema.ActualSchema
                            });
                            partSchema.Properties[field.Name] = propertySchema;
                        }
                    }

                    // Don't add "Part" here because users often create their own TypePart.
                    context.Document.Definitions[ctd.Name + _openApiOptions.ContentTypes.SchemaNameExtension] = partSchema;

                    // Add fieldpart as property.
                    var typePropertySchema = new JsonSchemaProperty
                    {
                        Type = JsonObjectType.Object,
                        IsNullableRaw = true
                    };

                    typePropertySchema.OneOf.Add(new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        Reference = partSchema.ActualSchema
                    });
                    typeSchema.Properties[typeFieldPartDefinition.Name] = typePropertySchema;
                }

                // Add all other parts
                var parts = ctd.Parts.Where(x => x.PartDefinition.Name != ctd.Name);
                foreach (var partDefinition in parts)
                {
                    // Lookup part definition.
                    if (context.Document.Definitions.TryGetValue(partDefinition.PartDefinition.Name + _openApiOptions.ContentTypes.SchemaNameExtension,
                        out var typePartSchema))
                    {
                        // Add part as property.
                        var propertySchema = new JsonSchemaProperty
                        {
                            Type = JsonObjectType.Object,
                            IsNullableRaw = true
                        };

                        propertySchema.OneOf.Add(new JsonSchema
                        {
                            Type = JsonObjectType.Object,
                            Reference = typePartSchema.ActualSchema
                        });
                        typeSchema.Properties[partDefinition.Name] = propertySchema;
                    }
                }

                // Add final definition.
                context.Document.Definitions[ctd.Name + _openApiOptions.ContentTypes.SchemaTypeNameExtension + _openApiOptions.ContentTypes.SchemaNameExtension] = typeSchema;

                // Add discriminator mappings to actual type schema
                typeDtoSchema.ActualTypeSchema.DiscriminatorObject.Mapping.Add(
                    ctd.Name + _openApiOptions.ContentTypes.SchemaTypeNameExtension + _openApiOptions.ContentTypes.SchemaNameExtension,
                    new JsonSchema
                    {
                        Reference = typeSchema
                    }
                );
            }
        }

        private static void AlterArrayPropertiesToContentItemDtoSchema(
            IReadOnlyDictionary<string, JsonSchemaProperty> properties,
            JsonSchema contentItemSchema,
            JsonSchema contentItemDtoSchema)
        {
            if (properties == null)
            {
                return;
            }

            foreach (var property in properties)
            {
                if (property.Value.Type == JsonObjectType.Array && property.Value.Item.Reference == contentItemSchema)
                {
                    property.Value.Item.Reference = contentItemDtoSchema;
                }
                if (property.Value.Type == JsonObjectType.Object &&
                    property.Value.AdditionalPropertiesSchema != null &&
                    property.Value.AdditionalPropertiesSchema.Type == JsonObjectType.Array &&
                    property.Value.AdditionalPropertiesSchema.Item.Reference == contentItemSchema
                    )
                {
                    property.Value.AdditionalPropertiesSchema.Item.Reference = contentItemDtoSchema;

                }

                AlterArrayPropertiesToContentItemDtoSchema(property.Value.ActualProperties, contentItemSchema, contentItemDtoSchema);
            }
        }

        private static void AlterSchemaDefinition(DocumentProcessorContext context, string definitionName, string schemaNameExtension)
        {
            var definition = context.Document.Definitions[definitionName];
            // TODO post configure action here.
            context.Document.Definitions.Remove(definitionName);
            context.Document.Definitions[definitionName + schemaNameExtension] = definition;
        }

        private static void InsertDtoReferenceSchema(JsonSchema schema, JsonSchema dtoSchema)
        {
            var firstAllOf = schema.AllOf.ElementAt(0);
            schema.AllOf.Remove(firstAllOf);
            var referenceSchema = new JsonSchema
            {
                Type = JsonObjectType.Object,
                Reference = dtoSchema.ActualSchema
            };
            schema.AllOf.Add(referenceSchema);
        }
    }
}
