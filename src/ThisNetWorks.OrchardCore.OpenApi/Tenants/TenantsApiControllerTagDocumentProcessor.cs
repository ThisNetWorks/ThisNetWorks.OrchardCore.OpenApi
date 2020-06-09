using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.TenantsApi
{
    public class TenantsApiControllerTagDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public TenantsApiControllerTagDocumentProcessor(IOptions<OpenApiOptions> options)
        {
            _openApiOptions = options.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            if (string.IsNullOrEmpty(_openApiOptions.TenantsApi.ApiTag))
            {
                return;
            }

            context.Document.Paths.AlterApiControllerTag("/api/tenants", _openApiOptions.TenantsApi.ApiTag);
        }
    }
}
