using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Html.Models;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.Tests.ContentManager;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public static class ArticleItemHelper
    {
        public static async Task<ContentItem> CreateArticleItem()
        {
            var contentItem = await TestContentManager.ContentManager.NewAsync("Article");
            contentItem.DisplayText = "foo";
    
            // This is just a creator
            contentItem.Alter<ContentPart>("Article", x =>
            {
                x.Alter<TextField>("Subtitle", x => x.Text = "subtitle");
            });
            contentItem.Alter<HtmlBodyPart>(x => x.Html = "html");

            return contentItem;
        }
    }
}
