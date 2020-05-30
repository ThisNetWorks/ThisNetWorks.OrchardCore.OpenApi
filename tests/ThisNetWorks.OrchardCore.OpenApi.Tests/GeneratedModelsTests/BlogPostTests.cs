using Newtonsoft.Json.Linq;
using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels.Models;
using ThisNetWorks.OrchardCore.OpenApi.Tests.ContentManager;
using Xunit;
using ContentItem = OrchardCore.ContentManagement.ContentItem;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.GeneratedModelsTests
{
    public class BlogPostTests
    {
        [Fact]
        public async Task ShouldCreateBlogPost()
        {
            var post = await BlogPostItemHelper.CreateBlogItem();
            var markdown = post.Content.MarkdownBodyPart.Markdown.ToString() as string;
            var listContentItemId = post.Content.ContainedPart.ListContentItemId.ToString() as string;
            Assert.Equal("markdown", markdown);
            Assert.Equal("blogid", listContentItemId);
        }

        [Fact]
        public async Task ShouldConvertBlogPostToDto()
        {
            var post = await BlogPostItemHelper.CreateBlogItem();
            var postDto = post.ToDto<BlogPostItemDto>();
            Assert.Equal("markdown", postDto.MarkdownBodyPart.Markdown);

            var containedPart = (postDto.AdditionalProperties["containedPart"] as JObject).ToObject<ContainedPart>();
            Assert.Equal("blogid", containedPart.ListContentItemId);
        }

        [Fact]
        public async Task ShouldAlterBlogPostFromDto()
        {
            var blogPost = await BlogPostItemHelper.CreateBlogItem();
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
        public async Task ShouldCreateBlogPostFromDto()
        {
            // Always use ContentManager.NewAsync() if inside site code
            // When using Content Api NewAsync() or BuildNewVersion() should be called.
            var blogPost = await TestContentManager.ContentManager.NewAsync("BlogPost");
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
