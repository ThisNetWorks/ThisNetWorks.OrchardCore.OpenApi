using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using Xunit;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.ContentDtoTests
{
    // TODO we can dump these the extension tests cover them.
    public class BarItemTests
    {
        [Fact]
        public void CreateBar()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var serialized = JObject.FromObject(contentItem);
            var deserialized = serialized.ToObject<ContentItem>();

            var email = deserialized.Content.Bar.Email.Text.ToString();

            Assert.Equal("test@bar.com", email);
        }

        [Fact]
        public void ToBarItem()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var serialized = JObject.FromObject(contentItem);
            var deserialized = serialized.ToObject<BarItemDto>();

            var email = deserialized.Bar.Email.Text.ToString();

            Assert.Equal("test@bar.com", email);
        }

        [Fact]
        public void EditedBarItem()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var serialized = JObject.FromObject(contentItem);
            var deserialized = serialized.ToObject<BarItemDto>();
            deserialized.Bar.Email.Text = "test@foo.com";

            var backToJContentItem = JObject.FromObject(deserialized);
            var backtoContentItem = backToJContentItem.ToObject<ContentItem>();

            var email = backtoContentItem.Content.Bar.Email.Text.ToString();

            Assert.Equal("test@foo.com", email);
        }

        [Fact]
        public void MergedBarItem()
        {
            var contentItem = BarItemDtoHelper.CreateBarItem();

            var serialized = JObject.FromObject(contentItem);
            var deserialized = serialized.ToObject<BarItemDto>();
            deserialized.Bar.Email.Text = "test@foo.com";

            contentItem.Merge(deserialized);

            var email = contentItem.Content.Bar.Email.Text.ToString();

            Assert.Equal("test@foo.com", email);
        }
    }
}
