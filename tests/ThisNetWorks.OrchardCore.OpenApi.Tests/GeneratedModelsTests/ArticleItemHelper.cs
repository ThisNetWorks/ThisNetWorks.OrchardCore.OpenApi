using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using OrchardCore.Html.Models;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public static class ArticleItemHelper
    {
        public static ContentItem CreateArticleItem()
        {
            var contentItem = new ContentItem
            {
                ContentType = "Article",
                ContentItemId = "articleid",
                ContentItemVersionId = "articleversionid",
                DisplayText = "foo"
            };
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
