using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using OrchardCore.Html.Models;
using OrchardCore.Markdown.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.Tests.ContentManager;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public static class BagItemHelper
    {
        public static async Task<ContentItem> CreateBagItem()
        {
            var fooItem = await TestContentManager.ContentManager.NewAsync("SampleFoo");

            fooItem.Alter<MarkdownBodyPart>(x => x.Markdown = "markdown");

            var barItem = await TestContentManager.ContentManager.NewAsync("SampleBar");
            barItem.Alter<HtmlBodyPart>(x => x.Html = "html");

            var contentItem = await TestContentManager.ContentManager.NewAsync("SampleBag");
            contentItem.DisplayText = "bag";

            // This is just a creator
            contentItem.Alter<BagPart>("Samples", x =>
            {
                x.ContentItems = new List<ContentItem>
                {
                    barItem,
                    fooItem
                };
            });

            return contentItem;
        }
    }
}
