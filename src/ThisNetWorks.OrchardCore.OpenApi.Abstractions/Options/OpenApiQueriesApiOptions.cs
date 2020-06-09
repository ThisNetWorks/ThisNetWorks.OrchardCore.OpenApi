namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiQueriesApiOptions
    {
        public bool AlterPathSchema { get; set; } = true;
        public string PostOperationId { get; set; } = "Api_Query_Post";
        public string GetOperationId { get; set; } = "Api_Query_Get";
        public string ApiTag { get; set; } = "Queries";
    }
}
