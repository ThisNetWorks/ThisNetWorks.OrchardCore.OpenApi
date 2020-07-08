using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using System;
using System.Collections.Generic;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.Tests.ContentDtoTests;
using Xunit;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.ContentDtoExtensionTests
{
    public class ContentDtoExtensionTests
    {
        [Fact]
        public void ToBarItem()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var dto = contentItem.ToDto<BarItemDto>();

            var email = dto.Bar.Email.Text.ToString();

            Assert.Equal("test@bar.com", email);
        }

        [Fact]
        public void MergedBarItem()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var dto = contentItem.ToDto<BarItemDto>();
            dto.Bar.Email.Text = "test@foo.com";

            contentItem.FromDto(dto);

            var email = contentItem.Content.Bar.Email.Text.ToString();

            Assert.Equal("test@foo.com", email);
        }

        [Fact]
        public void MergedBarItem2()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var dto = contentItem.ToDto<BarItemDto>();
            dto.Bar.Email.Text = "test@foo.com";

            var dtoJ = JObject.FromObject(dto);
            contentItem.Merge(dtoJ);

            var email = contentItem.Content.Bar.Email.Text.ToString();

            Assert.Equal("test@foo.com", email);
        }

        [Fact]
        public void ToBarContentItem()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var dto = contentItem.ToDto<BarItemDto>();

            var toContentItem = dto.ToContentItem();

            var email = toContentItem.Content.Bar.Email.Text.ToString();

            Assert.Equal("test@bar.com", email);
        }        
    }
}
