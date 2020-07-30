using System.Collections.Generic;

namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiContentTypeOptions
    {
        public bool ProcessContentTypes { get; set; } = true;
        public bool IncludeAllParts { get; set; } = true;
        public bool IncludeAllFields { get; set; } = true;

        /// <summary>
        /// Content types to exclude. Is case sensitive.
        /// </summary>
        public List<string> ExcludedTypes { get; } = new List<string>();
        /// <summary>
        /// Content fields to exclude. Is not case sensitive. 
        /// </summary>
        public List<string> ExcludedFields { get; } = new List<string>();

        /// <summary>
        /// Content types to exclude. Is case sensitive.
        /// </summary>
        public List<string> ExcludedParts { get; } = new List<string>();
        public List<string> TreatPartsAsDynamic { get; set; } = new List<string>();

        public string SchemaNameExtension { get; set; } = "Dto";
        public string SchemaTypeNameExtension { get; set; } = "Item";
    }
}
