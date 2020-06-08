using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ThisNetWorks.OrchardCore.OpenApi.ConsoleClient.Security
{
    public static class TokenExtensions
    {
        public static async Task<BearerToken> GetToken(this HttpClient client, string clientId, string clientSecret, string url)
        {
            string credentials = String.Format("{0}:{1}", clientId, clientSecret);

            //Define Headers
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials)));

            //Prepare Request Body
            List<KeyValuePair<string, string>> requestData = new List<KeyValuePair<string, string>>();
            requestData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            requestData.Add(new KeyValuePair<string, string>("scope", "openid profile roles"));

            FormUrlEncodedContent requestBody = new FormUrlEncodedContent(requestData);

            //Request Token
            var request = await client.PostAsync(url, requestBody);
            var response = await request.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<BearerToken>(response);
        }
    }
}
