using OrchardCore.ContentFields.Fields;
using OrchardCore.ContentManagement;
using ThisNetWorks.OrchardCore.OpenApi.Models;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.ContentDtoTests
{
    public class BarItemDto : ContentItemDto
    {
        public BarPartDto Bar { get; set; }
    }

    public class BarPartDto : ContentPartDto
    {
        // TODO textFieldDto
        public TextField Email { get; set; }
    }

    public static class BarItemDtoHelper
    {
        public static ContentItem CreateBarItem()
        {
            var contentItem = new ContentItem();
            // This is just a creator
            contentItem.Alter<ContentPart>("Bar", x =>
            {
                x.Alter<TextField>("Email", x => x.Text = "test@bar.com");
            });
            return contentItem;
        }
    }
}
