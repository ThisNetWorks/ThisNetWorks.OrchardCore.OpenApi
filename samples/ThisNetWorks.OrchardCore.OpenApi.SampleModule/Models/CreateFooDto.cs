using Newtonsoft.Json;
using NJsonSchema.Converters;
using System.Runtime.Serialization;

namespace ThisNetWorks.OrchardCore.OpenApi.SampleModule.Models
{
    [JsonConverter(typeof(JsonInheritanceConverter), "discriminator")]
    [KnownType(typeof(UpdateFooDto))]
    public class CreateFooDto
    {
        public string Text { get; set; }
    }

    public class UpdateFooDto : CreateFooDto
    {

    }
}
