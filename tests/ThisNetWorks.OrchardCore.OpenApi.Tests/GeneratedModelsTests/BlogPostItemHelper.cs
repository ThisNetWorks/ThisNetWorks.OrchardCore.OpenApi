using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using OrchardCore.Markdown.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public static class BlogPostItemHelper
    {
        public static ContentItem CreateBlogItem()
        {
            var contentItem = new ContentItem
            {
                ContentType = "BlogPost",
                ContentItemId = "blobpostid",
                ContentItemVersionId = "blogpostversionid",
                DisplayText = "foo"
            };
            // This is just a creator
            contentItem.Alter<MarkdownBodyPart>(x => x.Markdown = "markdown");
            contentItem.Weld<ContainedPart>();
            contentItem.Alter<ContainedPart>(x => x.ListContentItemId = "blogid");

            return contentItem;
        }
    }
}
