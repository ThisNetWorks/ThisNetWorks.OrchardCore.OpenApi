using Microsoft.Extensions.Options;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    public class ContentsApiControllerTagDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public ContentsApiControllerTagDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (string.IsNullOrEmpty(_openApiOptions.ContentsApi.ApiTag))
            {
                return;
            }

            // This alters the api/content path to a more useful path segment.
            context.Document.Paths.AlterApiControllerTag("/api/content", _openApiOptions.ContentsApi.ApiTag);
        }
    }
}
