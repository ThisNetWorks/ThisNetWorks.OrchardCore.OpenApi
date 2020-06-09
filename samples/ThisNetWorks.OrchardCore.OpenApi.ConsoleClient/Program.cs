using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ThisNetWorks.OrchardCore.OpenApi.ConsoleClient.Client;
using ThisNetWorks.OrchardCore.OpenApi.ConsoleClient.Security;

namespace ThisNetWorks.OrchardCore.OpenApi.ConsoleClient
{
    class Program
    {
        private static HttpClient HttpClient = new HttpClient();
        private static string LuceneQuery = "{\r\n  \"query\": {\r\n    \"term\": { \"Content.ContentItem.ContentType\": \"BlogPost\" }\r\n  },\r\n  \"sort\": {\r\n    \"Content.ContentItem.CreatedUtc\": {\r\n      \"order\": \"desc\",\r\n      \"type\": \"double\"\r\n    }\r\n  },\r\n  \"size\": 3\r\n}";

        static async Task Main(string[] args)
        {
            var client = new ContentClient(HttpClient);
            var token = await HttpClient.GetToken("sample_console_client", "developmentsecret", $"{client.BaseUrl}/connect/token");

            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var fooTextItemDto = (await client.Api_GetAsync("4qnhdhv3z54xk4fg4tdfke76c9")) as FooTextItemDto;

            Console.WriteLine("Reading from Api: " + fooTextItemDto.FooText.FooField.Text);

            fooTextItemDto.FooText.FooField.Text = "Foo field value - edited by api - " + Guid.NewGuid().ToString("n");

            fooTextItemDto = (await client.Api_PostAsync(false, fooTextItemDto)) as FooTextItemDto;

            Console.WriteLine("Written and read back from Api: " + fooTextItemDto.FooText.FooField.Text);

            Console.WriteLine(JsonConvert.SerializeObject(fooTextItemDto, Formatting.Indented));

            var queriesClient = new QueriesClient(HttpClient);

            var recentBlogPostsQuery = await queriesClient.Api_Query_PostAsync("RecentBlogPosts", String.Empty);

            foreach (var item in recentBlogPostsQuery.OfType<BlogPostItemDto>())
            {
                Console.WriteLine(item.DisplayText);
            }

            var aliasQuery = await queriesClient.Api_Query_GetAsync("AliasQuery", "{ alias:'categories' }");
            foreach (var item in aliasQuery)
            {
                Console.WriteLine("Sql query for aliases: " + item.DisplayText);
                Console.WriteLine(JsonConvert.SerializeObject(item, Formatting.Indented));
            }

            var luceneClient = new LuceneClient(HttpClient);

            // This style of lucene query will always return content items.
            var luceneContentQuery = await luceneClient.Api_Content_PostAsync("Search", LuceneQuery, String.Empty);
            foreach (var item in luceneContentQuery.Items.OfType<BlogPostItemDto>())
            {
                Console.WriteLine("Lucene query for blogposts: " + item.DisplayText);
            }

            // This style of query can also return any kind of document that is indexed with lucene.
            var luceneDocumentQuery = await luceneClient.Api_Documents_GetAsync("Search", LuceneQuery, null);
            foreach (var item in luceneDocumentQuery)
            {
                Console.WriteLine("Lucene document query: " + item.AdditionalProperties["Content.ContentItem.DisplayText"]);
            }

            var fooClient = new FooClient(HttpClient);
            var fooQuery = await fooClient.Foo_GetAllAsync();
            foreach (var item in fooQuery)
            {
                Console.WriteLine("Foo : " + item.Text);
            }
        }
    }
}
