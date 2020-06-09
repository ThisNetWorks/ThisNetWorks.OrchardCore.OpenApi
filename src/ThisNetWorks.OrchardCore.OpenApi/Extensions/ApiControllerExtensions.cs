using NSwag;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThisNetWorks.OrchardCore.OpenApi.Extensions
{
    public static class ApiControllerExtensions
    {
        public static void AlterApiControllerTag(this IDictionary<string, OpenApiPathItem> paths, string path, string tag)
        {
            var openApiPathItems = paths.Where(x => x.Key.StartsWith(path)).Select(x => x.Value);
            foreach (var pathItem in openApiPathItems)
            {
                foreach (var apiOperation in pathItem)
                {
                    if (apiOperation.Value.Tags.Any(x => string.Equals(x, "Api", StringComparison.OrdinalIgnoreCase)))
                    {
                        apiOperation.Value.Tags.Clear();
                        apiOperation.Value.Tags.Add(tag);
                    }
                }
            }
        }
    }
}
