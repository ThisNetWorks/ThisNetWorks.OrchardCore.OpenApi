using OrchardCore.ContentManagement.Handlers;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Handlers
{
    public class SamplePartHandler : ContentPartHandler<SamplePart>
    {
        public override Task InitializingAsync(InitializingContentContext context, SamplePart part)
        {
            part.Show = true;

            return Task.CompletedTask;
        }
    }
}