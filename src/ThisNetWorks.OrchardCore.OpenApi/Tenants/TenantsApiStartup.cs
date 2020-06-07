using Microsoft.Extensions.DependencyInjection;
using NSwag.Generation.Processors;
using OrchardCore.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThisNetWorks.OrchardCore.OpenApi.TenantsApi
{
    [RequireFeatures("OrchardCore.Tenants")]
    public class TenantsApiStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDocumentProcessor, TenantsApiControllerTagDocumentProcessor>();
        }
    }
}
