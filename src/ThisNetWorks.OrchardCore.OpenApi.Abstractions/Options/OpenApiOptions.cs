namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiOptions
    {
        public OpenApiContentTypeOptions ContentTypes { get; set; } = new OpenApiContentTypeOptions();
        public OpenApiContentsApiOptions ContentsApi { get; set; } = new OpenApiContentsApiOptions();
        public OpenApiMiddlewareOptions Middleware { get; set; } = new OpenApiMiddlewareOptions();
        public OpenApiPathOptions PathOptions { get; set; } = new OpenApiPathOptions();
    }
}
