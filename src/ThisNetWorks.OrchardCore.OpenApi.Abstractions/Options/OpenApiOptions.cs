using System.Collections;
using System.Collections.Generic;

namespace ThisNetWorks.OrchardCore.OpenApi.Options
{
    public class OpenApiOptions
    {
        public bool IncludeAllParts { get; set; } = true;
        public bool IncludeAllFields { get; set; } = true;

        /// <summary>
        /// Content types to exclude. Is case sensitive.
        /// </summary>
        public IList<string> ExcludedTypes { get; set; } = new List<string>();
        /// <summary>
        /// Content fields to exclude. Is not case sensitive. 
        /// </summary>
        public IList<string> ExcludedFields { get; set; } = new List<string>();

        /// <summary>
        /// Content types to exclude. Is case sensitive.
        /// </summary>
        public IList<string> ExcludedParts { get; set; } = new List<string>();

        public string SchemaNameExtension { get; set; } = "Dto";
        public string SchemaTypeNameExtension { get; set; } = "Item";
    }
}
