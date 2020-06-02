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

            var client = new ContentClient(HttpClient);

            var fooTextItemDto = (await client.Api_GetAsync("4qnhdhv3z54xk4fg4tdfke76c9")) as FooTextItemDto;

            Console.WriteLine("Reading from Api");
            Console.WriteLine(fooTextItemDto.FooText.FooField.Text);
            //Console.WriteLine(Prettify(blogPost.ToJson()));

            //blogPost.ContentType
            fooTextItemDto.FooText.FooField.Text = "Foo field value - edited by api - " + Guid.NewGuid().ToString("n");

            fooTextItemDto = (await client.Api_PostAsync(false, fooTextItemDto)) as FooTextItemDto;

            Console.WriteLine("Written and read back from Api");
            Console.WriteLine(fooTextItemDto.FooText.FooField.Text);
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
