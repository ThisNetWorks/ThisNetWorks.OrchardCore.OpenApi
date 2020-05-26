using OrchardCore.ContentManagement;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels;
using Xunit;
using ContentItem = OrchardCore.ContentManagement.ContentItem;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    // TODO when we are further through, take a copy of these models to test project and change the namespace.
    public class ArticleItemTests
    {
        [Fact]
        public void ShouldCreateArticle()
        {
            var articleItem = ArticleItemHelper.CreateArticleItem();

            var subtitle = articleItem.Content.Article.Subtitle.Text.ToString();
            var html = articleItem.Content.HtmlBodyPart.Html.ToString();
            Assert.Equal("foo", articleItem.DisplayText);
            Assert.Equal("subtitle", subtitle);
            Assert.Equal("html", html);
        }

        [Fact]
        public void ShouldConvertArticleToDto()
        {
            var articleItem = ArticleItemHelper.CreateArticleItem();
            var articleItemDto = articleItem.ToDto<ArticleItemDto>();
            Assert.Equal("foo", articleItemDto.DisplayText);
            Assert.Equal("subtitle", articleItemDto.Article.Subtitle.Text);
            Assert.Equal("html", articleItemDto.HtmlBodyPart.Html);
        }

        [Fact]
        public void ShouldAlterArticleFromDto()
        {
            var articleItem = ArticleItemHelper.CreateArticleItem();
            var articleItemDto = articleItem.ToDto<ArticleItemDto>();

            articleItemDto.DisplayText = "Foo";
            articleItemDto.Article.Subtitle.Text = "Subtitle";
            articleItemDto.HtmlBodyPart.Html = "Html";

            articleItem.FromDto(articleItemDto);

            var subtitle = articleItem.Content.Article.Subtitle.Text.ToString();
            var html = articleItem.Content.HtmlBodyPart.Html.ToString();
            Assert.Equal("Foo", articleItem.DisplayText);
            Assert.Equal("Subtitle", subtitle);
            Assert.Equal("Html", html);
        }

        [Fact]
        public void ShouldCreateArticleFromDto()
        {
            // Never do this. Always use ContentManager.NewAsync();
            var articleItem = new ContentItem();
            var articleItemDto = new ArticleItemDto
            {
                DisplayText = "Foo",
                Article = new ArticleDto
                {
                    Subtitle = new TextFieldDto
                    {
                        Text = "Subtitle"
                    }
                },
                HtmlBodyPart = new HtmlBodyPartDto
                {
                    Html = "Html"
                }
            };

            articleItem.FromDto(articleItemDto);

            var subtitle = articleItem.Content.Article.Subtitle.Text.ToString();
            var html = articleItem.Content.HtmlBodyPart.Html.ToString();
            Assert.Equal("Foo", articleItem.DisplayText);
            Assert.Equal("Subtitle", subtitle);
            Assert.Equal("Html", html);
        }
    }
}
