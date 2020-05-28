using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using System;
using System.Collections.Generic;
using System.Text;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels;
using Xunit;
using ContentItem = OrchardCore.ContentManagement.ContentItem;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public class BlogPostTests
    {
        [Fact]
        public void ShouldCreateBlogPost()
        {
            var post = BlogPostItemHelper.CreateBlogItem();
            var markdown = post.Content.MarkdownBodyPart.Markdown.ToString() as string;
            var listContentItemId = post.Content.ContainedPart.ListContentItemId.ToString() as string;
            Assert.Equal("markdown", markdown);
            Assert.Equal("blogid", listContentItemId);
        }

        [Fact]
        public void ShouldConvertBlogPostToDto()
        {
            var post = BlogPostItemHelper.CreateBlogItem();
            var postDto = post.ToDto<BlogPostItemDto>();
            Assert.Equal("markdown", postDto.MarkdownBodyPart.Markdown);

            var containedPart = (postDto.AdditionalProperties["containedPart"] as JObject).ToObject<ContainedPart>();
            Assert.Equal("blogid", containedPart.ListContentItemId);
        }

        [Fact]
        public void ShouldAlterBlogPostFromDto()
        {
            var blogPost = BlogPostItemHelper.CreateBlogItem();
            var blogPostItemDto = blogPost.ToDto<BlogPostItemDto>();

            var newContainedPart = new ContainedPartDto
            {
                ListContentItemId = "newid"
            };
            var jContainedPart = JObject.FromObject(newContainedPart);
            // This should change to 'containedPart' when we resolve 'code' content types.
            // i.e. types that are not list and are generally welded on.
            // TODO camelcase this in the converter?
            blogPostItemDto.AdditionalProperties["containedPart"] = jContainedPart;

            blogPost.FromDto(blogPostItemDto);

            var newListContentItemId = blogPost.Content.ContainedPart.ListContentItemId.ToString() as string;
            Assert.Equal("newid", newListContentItemId);
        }

        [Fact]
        public void ShouldCreateBlogPostFromDto()
        {
            // Never do this. Always use ContentManager.NewAsync();
            var blogPost = new ContentItem();
            var blogPostItemDto = new BlogPostItemDto
            {
                DisplayText = "Foo",
                MarkdownBodyPart = new MarkdownBodyPartDto
                {
                    Markdown = "markdown"
                }
            };

            var newContainedPart = new ContainedPartDto
            {
                ListContentItemId = "blogid"
            };
            var jContainedPart = JObject.FromObject(newContainedPart);
            // This should change to 'containedPart' when we resolve 'code' content types.
            // i.e. types that are not list and are generally welded on.
            blogPostItemDto.AdditionalProperties["containedPart"] = jContainedPart;

            blogPost.FromDto(blogPostItemDto);

            var markdown = blogPost.Content.MarkdownBodyPart.Markdown.ToString() as string;
            var listContentItemId = blogPost.Content.ContainedPart.ListContentItemId.ToString() as string;
            Assert.Equal("markdown", markdown);
            Assert.Equal("blogid", listContentItemId);
        }
    }
}
