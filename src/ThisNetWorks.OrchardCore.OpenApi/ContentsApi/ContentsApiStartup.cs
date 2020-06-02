using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;
using OrchardCore.Modules;

namespace ThisNetWorks.OrchardCore.OpenApi.ContentsApi
{
    [RequireFeatures("OrchardCore.Contents")]
    public class ContentsApiStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentProcessor, ContentsApiControllerTagDocumentProcessor>();
            services.AddSingleton<IDocumentProcessor, ContentsApiControllerDocumentProcessor>();
            services.AddSingleton<IDocumentProcessor, RemoveContentElementDocumentProcessor>();
        }
    }
}
