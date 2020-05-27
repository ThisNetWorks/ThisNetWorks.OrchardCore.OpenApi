using OrchardCore.ContentManagement;
using OrchardCore.Lists.Models;
using ThisNetWorks.OrchardCore.OpenApi.Models;
using ThisNetWorks.OrchardCore.OpenApi.SampleModels;
using Xunit;
using ContentItem = OrchardCore.ContentManagement.ContentItem;

namespace ThisNetWorks.OrchardCore.OpenApi.Tests.ExtendedPartialTests
{
    // TODO this needs some cleaning up (we want a containerPart dto), but proves a point.
    public class ExtendedPartialBlogPostTests
    {
        [Fact]
        public void ShouldCreateBlogPostFromExtendedDto()
        {
            // Never do this. Always use ContentManager.NewAsync();
            var blogPost = new ContentItem();
            var blogPostItemDto = new BlogPostItemDto
            {
                DisplayText = "Foo",
                MarkdownBodyPart = new MarkdownBodyPartDto
                {
                    Markdown = "markdown"
                },
                ContainedPart = new ContainedPart
                {
                    ListContentItemId = "blogid"
                }
            };

            blogPost.FromDto(blogPostItemDto);

            var markdown = blogPost.Content.MarkdownBodyPart.Markdown.ToString() as string;
            var listContentItemId = blogPost.Content.ContainedPart.ListContentItemId.ToString() as string;
            Assert.Equal("markdown", markdown);
            Assert.Equal("blogid", listContentItemId);
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class BlogPostDto : ContentPartDto
    {
        [Newtonsoft.Json.JsonProperty("subtitle", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TextFieldDto Subtitle { get; set; }

        [Newtonsoft.Json.JsonProperty("image", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public MediaFieldDto Image { get; set; }

        [Newtonsoft.Json.JsonProperty("tags", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TaxonomyFieldDto Tags { get; set; }

        [Newtonsoft.Json.JsonProperty("category", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TaxonomyFieldDto Category { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "10.1.15.0 (Newtonsoft.Json v11.0.0.0)")]
    public partial class BlogPostItemDto : ContentItemDto
    {
        [Newtonsoft.Json.JsonProperty("blogPost", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public BlogPostDto BlogPost { get; set; }

        [Newtonsoft.Json.JsonProperty("titlePart", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TitlePartDto TitlePart { get; set; }

        [Newtonsoft.Json.JsonProperty("autoroutePart", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public AutoroutePartDto AutoroutePart { get; set; }

        [Newtonsoft.Json.JsonProperty("markdownBodyPart", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public MarkdownBodyPartDto MarkdownBodyPart { get; set; }

        private System.Collections.Generic.IDictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

        public ContainedPart ContainedPart { get; set; }

    }
}
