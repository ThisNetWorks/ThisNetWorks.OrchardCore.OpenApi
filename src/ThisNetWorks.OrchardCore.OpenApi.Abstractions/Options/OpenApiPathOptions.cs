using System.Collections.Generic;

namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiPathOptions
    {
        public List<string> PathsToRemove { get; } = new List<string>();
    }
}
