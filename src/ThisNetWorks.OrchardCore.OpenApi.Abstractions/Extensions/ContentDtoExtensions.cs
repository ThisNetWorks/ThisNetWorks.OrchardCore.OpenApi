using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using ThisNetWorks.OrchardCore.OpenApi.Models;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;

namespace OrchardCore.ContentManagement
{
    public static class ContentDtoExtensions
    {
        private static readonly JsonMergeSettings JsonMergeSettings = new JsonMergeSettings
        {
            MergeArrayHandling = MergeArrayHandling.Replace,
            // TODO don't need this is we are pascal casing names first.
            PropertyNameComparison = StringComparison.OrdinalIgnoreCase
        };

        public static TDto ToDto<TDto>(this ContentElement content)
                where TDto : ContentElementDto
        {

            var serialized = JObject.FromObject(content);
            var deserialized = serialized.ToObject(typeof(TDto)) as TDto;
            return deserialized;
        }

        // TODO we don't have a merge for elements.
        public static ContentItem FromDto<TDto>(this ContentItem contentItem, TDto dto, JsonMergeSettings jsonMergeSettings = null)
            where TDto : ContentItemDto
        {
            if (dto == null)
            {
                throw new ArgumentNullException();
            }

            // TODO there must be a better way to do this.
            var jObject = JObject.FromObject(dto);
            jObject = jObject.ToPascalCase();
            if (jsonMergeSettings == null)
            {
                return contentItem.Merge(jObject, JsonMergeSettings);
            } else
            {
                return contentItem.Merge(jObject, jsonMergeSettings);
            }
        }
    }
}
