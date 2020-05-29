using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardCore.Markdown.Models;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.Tests.ContentManager;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public static class BlogPostItemHelper
    {
        public static async Task<ContentItem> CreateBlogItem()
        {
            var contentItem = await TestContentManager.ContentManager.NewAsync("BlogPost");
            contentItem.DisplayText = "foo";

            // This is just a creator
            contentItem.Alter<MarkdownBodyPart>(x => x.Markdown = "markdown");
            contentItem.Weld<ContainedPart>();
            contentItem.Alter<ContainedPart>(x => x.ListContentItemId = "blogid");

            return contentItem;
        }
    }
}
