using Microsoft.Extensions.Options;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Linq;
using ThisNetWorks.OrchardCore.OpenApi.Options;

namespace ThisNetWorks.OrchardCore.OpenApi.Processors
{
    public class RemoveControllersDocumentProcessor : IDocumentProcessor
    {
        private readonly OpenApiOptions _openApiOptions;

        public RemoveControllersDocumentProcessor(
            IOptions<OpenApiOptions> openApiOptions
            )
        {
            _openApiOptions = openApiOptions.Value;
        }

        public void Process(DocumentProcessorContext context)
        {
            foreach (var pathToRemove in _openApiOptions.PathsToRemove)
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
