using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;
using OrchardCore.Modules;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentTypes
{
    [RequireFeatures("OrchardCore.ContentTypes")]
    public class ContentTypesStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentProcessor, ContentTypeDocumentProcessor>();
        }
    }
}
