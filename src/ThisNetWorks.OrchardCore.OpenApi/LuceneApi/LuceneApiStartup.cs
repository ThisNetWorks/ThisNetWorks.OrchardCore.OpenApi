using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;
using OrchardCore.Modules;

namespace ThisNetWorks.OrchardCore.OpenApi.LuceneApi
{
    [RequireFeatures("OrchardCore.Lucene")]
    public class LuceneApiStartup : StartupBase
    {
        public override int Order => 100;
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentProcessor, LuceneApiControllerTagDocumentProcessor>();
            services.AddSingleton<IDocumentProcessor, LuceneApiControllerDocumentProcessor>();
        }
    }
}
