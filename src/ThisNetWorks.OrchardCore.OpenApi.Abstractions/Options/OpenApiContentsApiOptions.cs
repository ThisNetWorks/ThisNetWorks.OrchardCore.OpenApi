namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiContentsApiOptions
    {
        public bool RemoveContentElements { get; set; } = true;
        public bool AlterPathSchema { get; set; } = true;
        public string ApiTag { get; set; } = "Content";
    }
}
