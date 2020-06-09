namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiLuceneApiOptions
    {
        public bool AlterPathSchema { get; set; } = true;
        public string ContentPostOperationId { get; set; } = "Api_Content_Post";
        public string ContentGetOperationId { get; set; } = "Api_Content_Get";
        public string DocumentsPostOperationId { get; set; } = "Api_Documents_Post";
        public string DocumentsGetOperationId { get; set; } = "Api_Documents_Get";
        public string ApiTag { get; set; } = "Lucene";
    }
}
