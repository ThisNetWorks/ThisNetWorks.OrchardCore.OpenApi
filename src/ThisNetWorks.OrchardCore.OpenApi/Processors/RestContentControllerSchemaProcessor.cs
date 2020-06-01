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

            var contentItemDtoSchema = context.Document.Definitions["ContentItemDto"];

            var path = context.Document.Paths["/api/restcontent/{contentItemId}"];
            OpenApiOperation restGet = path["get"];
            OpenApiResponse restGetResponse = restGet.Responses["200"];
            ApplyContentItemDtoToContent(contentItemDtoSchema, restGetResponse.Content);

            OpenApiOperation restDelete = path["delete"];
            OpenApiResponse restDeleteResponse = restDelete.Responses["200"];
            ApplyContentItemDtoToContent(contentItemDtoSchema, restDeleteResponse.Content);

            path = context.Document.Paths["/api/restcontent"];
            OpenApiOperation restPost = path["post"];
            OpenApiResponse restPostResponse = restPost.Responses["200"];
            ApplyContentItemDtoToContent(contentItemDtoSchema, restPostResponse.Content);

            //var body = restPost.RequestBody.Content


            ApplyContentItemDtoToContent(contentItemDtoSchema, restPost.RequestBody.Content);
        }

        private static void ApplyContentItemDtoToContent(JsonSchema contentItemDtoSchema, IDictionary<string, OpenApiMediaType> content)
        {
            var firstContentMediaType = content.FirstOrDefault();

            if (!String.IsNullOrEmpty(firstContentMediaType.Key))
            {
                content.Remove(firstContentMediaType.Key);
                content["application/json"] = new OpenApiMediaType
                {
                    Schema = new JsonSchema
                    {
                        Reference = contentItemDtoSchema
                    }
                };
            };
        }
    }
}
