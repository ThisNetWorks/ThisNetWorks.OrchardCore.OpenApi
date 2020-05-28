using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels;
using Xunit;
using ContentItem = OrchardCore.ContentManagement.ContentItem;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public class BagItemTests
    {
        [Fact]
        public void ShouldCreateBag()
        {
            var bagItem = BagItemHelper.CreateBagItem();

            var bagItems = bagItem.Content.Samples.ContentItems.ToObject<List<ContentItem>>() as List<ContentItem>;
            var fooItem = bagItems.FirstOrDefault(x => x.ContentType == "SampleFoo");
            var html = fooItem.Content.MarkdownBodyPart.Markdown.ToString();
            Assert.Equal("markdown", html);

            var barItem = bagItems.FirstOrDefault(x => x.ContentType == "SampleBar");
            var markdown = barItem.Content.HtmlBodyPart.Html.ToString();
            Assert.Equal("html", markdown);
        }

        [Fact]
        public void ShouldConvertBagToDto()
        {
            var bagItem = BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();

            Assert.Equal(2, bagDto.Samples.ContentItems.Count);

        }

        [Fact]
        public void ShouldConvertBarToDto()
        {
            var bagItem = BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();
            var contentItemDto = bagDto.Samples.ContentItems.FirstOrDefault(x => x.ContentType == "SampleBar");
            var barItemDto = contentItemDto.ToDto<SampleBarItemDto>();
            Assert.Equal("html", barItemDto.HtmlBodyPart.Html);

        }

        [Fact]
        public void ShouldConvertBarsToDtos()
        {
            var bagItem = BagItemHelper.CreateBagItem();
            var bagDto = bagItem.ToDto<SampleBagItemDto>();
            var barItems = bagDto.Samples.ContentItems.OfDtoType<SampleBarItemDto>();
            Assert.Single(barItems);

        }
    }
}
