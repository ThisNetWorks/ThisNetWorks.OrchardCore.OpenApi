using OrchardCore.ContentManagement;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels;
using ThisNetWorks.OrchardCore.OpenApi.Tests.ContentManager;
using Xunit;
using ContentItem = OrchardCore.ContentManagement.ContentItem;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    // TODO when we are further through, take a copy of these models to test project and change the namespace.
    public class ArticleItemTests
    {
        [Fact]
        public async Task ShouldCreateArticle()
        {
            var articleItem = await ArticleItemHelper.CreateArticleItem();

            var subtitle = articleItem.Content.Article.Subtitle.Text.ToString();
            var html = articleItem.Content.HtmlBodyPart.Html.ToString();
            Assert.Equal("foo", articleItem.DisplayText);
            Assert.Equal("subtitle", subtitle);
            Assert.Equal("html", html);
        }

        [Fact]
        public async Task ShouldConvertArticleToDto()
        {
            var articleItem = await ArticleItemHelper.CreateArticleItem();
            var articleItemDto = articleItem.ToDto<ArticleItemDto>();
            Assert.Equal("foo", articleItemDto.DisplayText);
            Assert.Equal("subtitle", articleItemDto.Article.Subtitle.Text);
            Assert.Equal("html", articleItemDto.HtmlBodyPart.Html);
        }

        [Fact]
        public async Task ShouldAlterArticleFromDto()
        {
            var articleItem = await ArticleItemHelper.CreateArticleItem();
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
        public async Task ShouldCreateArticleFromDto()
        {
            var articleItem = await TestContentManager.ContentManager.NewAsync("Article");
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
