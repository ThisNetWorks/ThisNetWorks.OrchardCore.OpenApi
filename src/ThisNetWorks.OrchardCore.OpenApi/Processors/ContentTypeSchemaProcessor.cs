using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.Mvc.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Models;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.Processors
{
    public class ContentTypeSchemaProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;
        private readonly ContentOptions _contentOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IContentDefinitionManager _contentDefinitionManager;

        public ContentTypeSchemaProcessor(
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
            _contentDefinitionManager ??= _httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IContentDefinitionManager>();

            var contentElementDtoSchema = context.SchemaGenerator.Generate(typeof(ContentElementDto), context.SchemaResolver);
            contentElementDtoSchema.AllowAdditionalProperties = true;
            var fieldDtoSchema = context.SchemaGenerator.Generate(typeof(ContentFieldDto), context.SchemaResolver);
            fieldDtoSchema.AllowAdditionalProperties = true;

            var ctds = _contentDefinitionManager.ListTypeDefinitions();
            var allFieldDefinitions = ctds
                .SelectMany(x => x.Parts.SelectMany(p => p.PartDefinition.Fields))
                .Select(x => x.FieldDefinition).ToLookup(x => x.Name, StringComparer.OrdinalIgnoreCase);

            // Process fields
            foreach (var contentFieldOption in _contentOptions.ContentFieldOptions)
            {
                if (!_openApiOptions.IncludeAllFields)
                {
                    if (!allFieldDefinitions.Contains(contentFieldOption.Type.Name))
                    {
                        continue;
                    }
                }

                if (_openApiOptions.ExcludedFields.Any(x => string.Equals(x, contentFieldOption.Type.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var fieldSchema = context.SchemaGenerator.Generate(contentFieldOption.Type, context.SchemaResolver);
                fieldSchema.AllOf.ElementAt(1).AllowAdditionalProperties = true;
                // remove first AllOf and reinsert the fieldDtoSchema as the ref.
                InsertDtoReferenceSchema(fieldSchema, fieldDtoSchema);

                // Change schema regisitration name to 'TextFieldDto'               
                AlterSchemaDefinition(context, contentFieldOption.Type.Name, _openApiOptions.SchemaNameExtension);
            }

            // Process Parts
            // Note we have to use the content definition manager here
            // because parts may be created dynamically.
            // But we will also have to include definitions for code only parts, like metadata
            // And somewhere we will have to allow for additional properties, so they are serialized.

            var partDtoSchema = context.SchemaGenerator.Generate(typeof(ContentPartDto), context.SchemaResolver);
            partDtoSchema.AllowAdditionalProperties = true;
            var allPartDefinitions = _contentDefinitionManager.ListPartDefinitions();
            var typedPartDefinitions = _contentOptions.ContentPartOptionsLookup;
            foreach (var partDefinition in allPartDefinitions)
            {
                // check to see if it is a hard typed part.
                if (_contentOptions.ContentPartOptionsLookup.TryGetValue(partDefinition.Name, out var contentPartOption))
                {
                    var partSchema = context.SchemaGenerator.Generate(contentPartOption.Type, context.SchemaResolver);

                    partSchema.AllOf.ElementAt(1).AllowAdditionalProperties = true;
                    // remove first AllOf and reinsert the partDtoSchema as the ref.
                    InsertDtoReferenceSchema(partSchema, partDtoSchema);

                    //TODO test this, it's being added as a direct property
                    // but the other properties are added as part of all of
                    // so we might need to find all of index 1 and insert it there
                    foreach (var field in partDefinition.Fields)
                    {
                        // Lookup field definition.
                        if (context.Document.Definitions.TryGetValue(field.FieldDefinition.Name + _openApiOptions.SchemaNameExtension, out var fieldSchema))
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
                            partSchema.Properties[field.Name.ToCamelCase()] = propertySchema;
                        }
                    }

                    // Change schema regisitration name to 'HtmlPartDto'
                    AlterSchemaDefinition(context, contentPartOption.Type.Name, _openApiOptions.SchemaNameExtension);

                }
                // TODO account for dynamic parts.
                // TODO account for code only parts (and additional properties).
                // wait till we have some testing code in place to see how the additional properties
                // will serialize.
            }

            // Content Types

            var typeDtoSchema = context.SchemaGenerator.Generate(typeof(ContentItemDto), context.SchemaResolver);
            typeDtoSchema.AllowAdditionalProperties = true;
            foreach (var ctd in ctds)
            {
                if (_openApiOptions.ExcludedTypes.Any(x => string.Equals(x, ctd.Name)))
                {
                    continue;
                }
                var typeReferenceSchema = new JsonSchema
                {
                    Type = JsonObjectType.Object,
                    Reference = typeDtoSchema.ActualSchema,
                    AllowAdditionalProperties = true
                };

                var typeSchema = new JsonSchema
                {
                    Type = JsonObjectType.Object,
                    AllowAdditionalProperties = true
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
                        AllowAdditionalProperties = true
                    };
                    var partReferenceSchema = new JsonSchema
                    {
                        Type = JsonObjectType.Object,
                        AllowAdditionalProperties = true,
                        Reference = partDtoSchema.ActualSchema
                    };
                    partSchema.AllOf.Add(partReferenceSchema);
                    foreach (var field in typeFieldPartDefinition.PartDefinition.Fields)
                    {
                        // Lookup field definition.
                        if (context.Document.Definitions.TryGetValue(field.FieldDefinition.Name + _openApiOptions.SchemaNameExtension, out var fieldSchema))
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
                            partSchema.Properties[field.Name.ToCamelCase()] = propertySchema;
                        }
                    }

                    // Don't add "Part" here because users often create their own TypePart.
                    context.Document.Definitions[ctd.Name + _openApiOptions.SchemaNameExtension] = partSchema;

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
                    typeSchema.Properties[typeFieldPartDefinition.Name.ToCamelCase()] = typePropertySchema;
                }

                // Add all other parts
                var parts = ctd.Parts.Where(x => x.PartDefinition.Name != ctd.Name);
                foreach (var partDefinition in parts)
                {
                    // Lookup part definition.
                    if (context.Document.Definitions.TryGetValue(partDefinition.PartDefinition.Name + _openApiOptions.SchemaNameExtension,
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
                        typeSchema.Properties[partDefinition.Name.ToCamelCase()] = propertySchema;
                    }
                }


                context.Document.Definitions[ctd.Name + _openApiOptions.SchemaTypeNameExtension + _openApiOptions.SchemaNameExtension] = typeSchema;
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
