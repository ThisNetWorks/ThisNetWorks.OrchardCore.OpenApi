using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;
using OrchardCore.Modules;

namespace ThisNetWorks.OrchardCore.OpenApi.QueriesApi
{
    [RequireFeatures("OrchardCore.Queries")]
    public class QueriesApiStartup : StartupBase
    {
        public override int Order => 100;
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentProcessor, QueriesApiControllerDocumentProcessor>();
            services.AddSingleton<IDocumentProcessor, QueriesApiControllerTagDocumentProcessor>();
        }
    }
}
