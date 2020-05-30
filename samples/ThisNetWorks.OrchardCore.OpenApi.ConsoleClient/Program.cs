using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

            var client = new RestContentClient(HttpClient);

            var blogPost = (await client.RestContent_GetAsync("42fkrdamrpyp02jz9ye9vjtkce")) as BlogPostItemDto;

            Console.WriteLine(blogPost.MarkdownBodyPart.Markdown);
            Console.WriteLine(Prettify(blogPost.ToJson()));
        }

        public static string Prettify(string jsonString)
        {
            using var stream = new MemoryStream();
            var document = JsonDocument.Parse(jsonString);
            var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
            document.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
