using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;
using OrchardCore.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThisNetWorks.OrchardCore.OpenApi.LuceneApi
{
    [RequireFeatures("OrchardCore.Lucene")]
    public class LuceneApiStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentProcessor, LuceneApiControllerTagDocumentProcessor>();
            services.AddSingleton<IDocumentProcessor, LuceneApiControllerDocumentProcessor>();
        }
    }
}
