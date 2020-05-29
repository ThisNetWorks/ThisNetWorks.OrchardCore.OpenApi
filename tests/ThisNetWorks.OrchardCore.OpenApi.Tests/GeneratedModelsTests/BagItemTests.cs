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

            var bagItems = bagItem.Content.Samples.ContentItems.ToObject<List<ContentItem>>() as List<ContentItem>;
            var fooItem = bagItems.FirstOrDefault(x => x.ContentType == "SampleFoo");
            var html = fooItem.Content.MarkdownBodyPart.Markdown.ToString();
            Assert.Equal("markdown", html);

            var barItem = bagItems.FirstOrDefault(x => x.ContentType == "SampleBar");
            var markdown = barItem.Content.HtmlBodyPart.Html.ToString();
            Assert.Equal("html", markdown);
        }

        [Fact]
        public async Task ShouldConvertBagToDto()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();

            Assert.Equal(2, bagDto.Samples.ContentItems.Count);
        }

        [Fact]
        public async Task ShouldConvertBarToDto()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();
            var contentItemDto = bagDto.Samples.ContentItems.FirstOrDefault(x => x.ContentType == "SampleBar");
            var barItemDto = contentItemDto.ToDto<SampleBarItemDto>();
            Assert.Equal("html", barItemDto.HtmlBodyPart.Html);
        }

        [Fact]
        public async Task ShouldConvertBarsToDtos()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();
            var barItems = bagDto.Samples.ContentItems.OfDtoType<SampleBarItemDto>();
            Assert.Single(barItems);
        }

        [Fact]
        public async Task ShouldAlterBarItemFromDto()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();
            var barItem = bagDto.Samples.ContentItems.OfDtoType<SampleBarItemDto>().FirstOrDefault();
            barItem.HtmlBodyPart.Html = "altered";

            bagItem.FromDto(bagDto);

            var html = bagItem.Content.Samples.ContentItems[0].HtmlBodyPart.Html.ToString();
            Assert.Equal("altered", html);
        }

        [Fact]
        public async Task ShouldAlterBarItemAsSameObjectReference()
        {
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();
            var barItem = bagDto.Samples.ContentItems.OfDtoType<SampleBarItemDto>().FirstOrDefault();
            barItem.HtmlBodyPart.Html = "altered";
            bagItem.FromDto(bagDto);
            var html = bagItem.Content.Samples.ContentItems[0].HtmlBodyPart.Html.ToString();
            Assert.Equal("altered", html);
        }

        [Fact]
        public async Task ShouldCreateBagFromDto()
        {
            // Never do this. Always use ContentManager.NewAsync();
            var bagItem = await TestContentManager.ContentManager.NewAsync("SampleBag");

            var bagItemDto = new SampleBagItemDto
            {
                Samples = new BagPartDto
                {
                    ContentItems = new List<ContentItemDto>
                    {
                        await TestContentManager.ContentManager
                            .NewDtoAsync<SampleBarItemDto>("SampleBar", ci =>
                            {
                                ci.HtmlBodyPart = new HtmlBodyPartDto
                                {
                                    Html = "html"
                                };
                            }),
                        await TestContentManager.ContentManager
                            .NewDtoAsync<SampleFooItemDto>("SampleFoo", ci =>
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

            var html = bagItem.Content.Samples.ContentItems[0].HtmlBodyPart.Html.ToString();
            Assert.Equal("html", html);

            var markdown = bagItem.Content.Samples.ContentItems[1].MarkdownBodyPart.Markdown.ToString();
            Assert.Equal("markdown", markdown);
        }

        [Fact]
        public async Task ShouldAlterBagItemFromDto()
        {
            // Never do this. Always use ContentManager.NewAsync();
            var bagItem = await BagItemHelper.CreateBagItem();
            var bagItemDto = bagItem.ToDto<SampleBagItemDto>();
            bagItemDto.Samples.ContentItems.Add(
                await TestContentManager.ContentManager
                    .NewDtoAsync<SampleFooItemDto>("SampleFoo", ci =>
                    {
                        ci.MarkdownBodyPart = new MarkdownBodyPartDto
                        {
                            Markdown = "markdown"
                        };
                    })
            );

            bagItem.FromDto(bagItemDto);

            var markdown = bagItem.Content.Samples.ContentItems[2].MarkdownBodyPart.Markdown.ToString();
            Assert.Equal("markdown", markdown);
        }
    }
}
