using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.Processors
{
    public class ControllerSchemaProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;
        private readonly ContentOptions _contentOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private IContentDefinitionManager _contentDefinitionManager;

        public ControllerSchemaProcessor(
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
            foreach(var pathToRemove in _openApiOptions.PathsToRemove)
            {

                var paths = context.Document.Paths.Where(x => x.Key.Contains(pathToRemove));
                foreach (var path in paths.Select(x => x.Key))
                {
                    context.Document.Paths.Remove(path);
                }
            }
        }
    }
}
