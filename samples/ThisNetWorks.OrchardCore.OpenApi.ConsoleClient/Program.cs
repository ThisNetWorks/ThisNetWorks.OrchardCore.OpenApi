using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ThisNetWorks.OrchardCore.OpenApi.ConsoleClient.Client;

namespace ThisNetWorks.OrchardCore.OpenApi.ConsoleClient
{
    class Program
    {
        private static HttpClient HttpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var client = new ContentClient(HttpClient);

            var fooTextItemDto = (await client.Api_GetAsync("4qnhdhv3z54xk4fg4tdfke76c9")) as FooTextItemDto;

            Console.WriteLine("Reading from Api");
            Console.WriteLine(fooTextItemDto.FooText.FooField.Text);

            fooTextItemDto.FooText.FooField.Text = "Foo field value - edited by api - " + Guid.NewGuid().ToString("n");

            fooTextItemDto = (await client.Api_PostAsync(false, fooTextItemDto)) as FooTextItemDto;

            Console.WriteLine("Written and read back from Api");
            Console.WriteLine(fooTextItemDto.FooText.FooField.Text);

            //Console.WriteLine(JsonConvert.SerializeObject(fooTextItemDto, Formatting.Indented));

            var queriesClient = new QueriesClient(HttpClient);

            var recentBlogPostsQuery = await queriesClient.Api_Query_PostAsync("RecentBlogPosts", String.Empty);

            foreach(var item in recentBlogPostsQuery.OfType<BlogPostItemDto>())
            {
                Console.WriteLine(item.DisplayText);
            }
            //var fooClient = new FooClient(HttpClient);
            //var fooGet = await fooClient.Foo_GetAsync();

        }
    }
}
