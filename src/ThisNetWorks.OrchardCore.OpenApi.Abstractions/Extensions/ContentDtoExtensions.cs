using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using ThisNetWorks.OrchardCore.OpenApi.Models;

namespace OrchardCore.ContentManagement
{
    public static class ContentDtoExtensions
    {
        public static TDto ToDto<TDto>(this ContentElement content)
            where TDto : ContentElementDto
        {

            var serialized = JObject.FromObject(content);
            var deserialized = serialized.ToObject(typeof(TDto)) as TDto;
            return deserialized;
        }

        // TODO we don't have a merge for elements.
        public static ContentItem FromDto<TDto>(this ContentItem contentItem, TDto dto)
            where TDto : ContentItemDto
        {
            return contentItem.Merge(dto);
        }
    }
}
