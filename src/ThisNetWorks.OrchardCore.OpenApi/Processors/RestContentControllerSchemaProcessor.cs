using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.Processors
{
    public class RestContentControllerSchemaProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;
        private readonly ContentOptions _contentOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IContentDefinitionManager _contentDefinitionManager;

        public RestContentControllerSchemaProcessor(
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
            {
                OpenApiPathItem r = context.Document.Paths["/api/foo"];
                OpenApiOperation rGet = r["get"];
                OpenApiResponse rGetResponse = rGet.Responses["200"];
                JsonSchema rGetSchema = rGetResponse.Schema;
            }

            OpenApiPathItem rest = context.Document.Paths["/api/restcontent/{contentItemId}"];
            OpenApiOperation restGet = rest["get"];
            OpenApiResponse restGetResponse = restGet.Responses["200"];
            var firstContent = restGetResponse.Content.FirstOrDefault();

            var contentItemDtoSchema = context.Document.Definitions["ContentItemDto"];

            if (!String.IsNullOrEmpty(firstContent.Key)) 
            {
                restGetResponse.Content.Remove(firstContent.Key);
                //restGetResponse.Content["application/json"] = new OpenApiMediaType
                //{
                //    Schema = contentItemDtoSchema.ActualTypeSchema
                //};

                restGetResponse.Content["application/json"] = new OpenApiMediaType
                {
                    Schema = new JsonSchema
                    {
                        Reference = contentItemDtoSchema
                    }
                };
            };
            JsonSchema restGetSchema = restGetResponse.Schema;

            var ctds = _contentDefinitionManager.ListTypeDefinitions();
            // Goal.
            // Change return operation to a multiple / allof 
            // so for this we need every content type definition created.
            //foreach (var ctd in ctds)
            //{
            //    if (_openApiOptions.ExcludedTypes.Any(x => string.Equals(x, ctd.Name)))
            //    {
            //        continue;
            //    }
            //    if (context.Document.Definitions.TryGetValue(ctd.Name + _openApiOptions.SchemaTypeNameExtension + _openApiOptions.SchemaNameExtension, out var typeSchema))
            //    {
            //        var referenceSchema = new JsonSchema
            //        {
            //            Reference = typeSchema
            //        };
            //        restGetSchema.AnyOf.Add(referenceSchema);

            //    }

            //}
        }
    }
}
