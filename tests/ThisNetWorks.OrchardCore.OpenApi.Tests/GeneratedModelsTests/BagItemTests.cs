using OrchardCore.ContentManagement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.Extensions;
using ThisNetWorks.OrchardCore.OpenApi.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels;
using ThisNetWorks.OrchardCore.OpenApi.Tests.ContentManager;
using Xunit;
using ContentItem = OrchardCore.ContentManagement.ContentItem;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public class BagItemTests
    {
        [Fact]
        public async Task ShouldCreateBag()
        {
            var bagItem = await BagItemHelper.CreateBagItem();

            var bagItems = bagItem.Content.Items.ContentItems.ToObject<List<ContentItem>>() as List<ContentItem>;
            var fooItem = bagItems.FirstOrDefault(x => x.ContentType == "Foo");
            var html = fooItem.Content.MarkdownBodyPart.Markdown.ToString();
            Assert.Equal("markdown", html);

            var barItem = bagItems.FirstOrDefault(x => x.ContentType == "Bar");
            var markdown = barItem.Content.HtmlBodyPart.Html.ToString();
            Assert.Equal("html", markdown);
        }

        [Fact]
        public async Task ShouldConvertBagToDto()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagItemDto = bagItem.ToDto<BagItemDto>();

            Assert.Equal(2, bagItemDto.Items.ContentItems.Count);
        }

        [Fact]
        public async Task ShouldConvertBarToDto()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagItemDto = bagItem.ToDto<BagItemDto>();
            var contentItemDto = bagItemDto.Items.ContentItems.FirstOrDefault(x => x.ContentType == "Bar");
            var barItemDto = contentItemDto.ToDto<BarItemDto>();
            Assert.Equal("html", barItemDto.HtmlBodyPart.Html);
        }

        [Fact]
        public async Task ShouldConvertBarsToDtos()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagItemDto = bagItem.ToDto<BagItemDto>();
            var barItems = bagItemDto.Items.ContentItems.OfDtoType<BarItemDto>();
            Assert.Single(barItems);
        }

        [Fact]
        public async Task ShouldAlterBarItemFromDto()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagItemDto = bagItem.ToDto<BagItemDto>();
            var barItemDto = bagItemDto.Items.ContentItems.OfDtoType<BarItemDto>().FirstOrDefault();
            barItemDto.HtmlBodyPart.Html = "altered";

            bagItem.FromDto(bagItemDto);

            var html = bagItem.Content.Items.ContentItems[0].HtmlBodyPart.Html.ToString();
            Assert.Equal("altered", html);
        }

        [Fact]
        public async Task ShouldAlterBarItemAsSameObjectReference()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagItemDto = bagItem.ToDto<BagItemDto>();
            var barItemDto = bagItemDto.Items.ContentItems.OfDtoType<BarItemDto>().FirstOrDefault();
            barItemDto.HtmlBodyPart.Html = "altered";
            bagItem.FromDto(bagItemDto);
            var html = bagItem.Content.Items.ContentItems[0].HtmlBodyPart.Html.ToString();
            Assert.Equal("altered", html);
        }

        [Fact]
        public async Task ShouldCreateBagFromDto()
        {
            // Never do this. Always use ContentManager.NewAsync();
            var bagItem = await TestContentManager.ContentManager.NewAsync("Bag");

            var bagItemDto = new BagItemDto
            {
                Items = new BagPartDto
                {
                    ContentItems = new List<ContentItemDto>
                    {
                        await TestContentManager.ContentManager
                            .NewDtoAsync<BarItemDto>("Bar", ci =>
                            {
                                ci.HtmlBodyPart = new HtmlBodyPartDto
                                {
                                    Html = "html"
                                };
                            }),
                        await TestContentManager.ContentManager
                            .NewDtoAsync<FooItemDto>("Foo", ci =>
                            {
                                ci.MarkdownBodyPart = new MarkdownBodyPartDto
                                {
                                    Markdown = "markdown"
                                };
                            })
                    }
                }
            };

            bagItem.FromDto(bagItemDto);

            var html = bagItem.Content.Items.ContentItems[0].HtmlBodyPart.Html.ToString();
            Assert.Equal("html", html);

            var markdown = bagItem.Content.Items.ContentItems[1].MarkdownBodyPart.Markdown.ToString();
            Assert.Equal("markdown", markdown);
        }

        [Fact]
        public async Task ShouldAlterBagItemFromDto()
        {
            // Never do this. Always use ContentManager.NewAsync();
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagItemDto = bagItem.ToDto<BagItemDto>();
            bagItemDto.Items.ContentItems.Add(
                await TestContentManager.ContentManager
                    .NewDtoAsync<FooItemDto>("Foo", ci =>
                    {
                        ci.MarkdownBodyPart = new MarkdownBodyPartDto
                        {
                            Markdown = "markdown"
                        };
                    })
            );

            bagItem.FromDto(bagItemDto);

            var markdown = bagItem.Content.Items.ContentItems[2].MarkdownBodyPart.Markdown.ToString();
            Assert.Equal("markdown", markdown);
        }
    }
}
