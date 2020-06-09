namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiOptions
    {
        public OpenApiContentTypeOptions ContentTypes { get; set; } = new OpenApiContentTypeOptions();
        public OpenApiContentsApiOptions ContentsApi { get; set; } = new OpenApiContentsApiOptions();
        public OpenApiLuceneApiOptions LuceneApi { get; set; } = new OpenApiLuceneApiOptions();
        public OpenApiQueriesApiOptions QueriesApi { get; set; } = new OpenApiQueriesApiOptions();
        public OpenApiTenantsApiOptions TenantsApi { get; set; } = new OpenApiTenantsApiOptions();
        public OpenApiMiddlewareOptions Middleware { get; set; } = new OpenApiMiddlewareOptions();
        public OpenApiPathOptions PathOptions { get; set; } = new OpenApiPathOptions();
    }
}
