using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using OrchardCore.Html.Models;
using OrchardCore.Markdown.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public static class BagItemHelper
    {
        public static ContentItem CreateBagItem()
        {
            var fooItem = new ContentItem
            {
                ContentType = "SampleFoo"
            };

            fooItem.Alter<MarkdownBodyPart>(x => x.Markdown = "markdown");

            var barItem = new ContentItem
            {
                ContentType = "SampleBar"
            };
            barItem.Alter<HtmlBodyPart>(x => x.Html = "html");

            var contentItem = new ContentItem
            {
                ContentType = "SampleBag",
                ContentItemId = "bagid",
                ContentItemVersionId = "bagversionid",
                DisplayText = "bag"
            };
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
