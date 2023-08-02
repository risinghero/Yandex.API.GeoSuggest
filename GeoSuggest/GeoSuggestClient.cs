using GeoSuggest;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Yandex.API
{
    public class GeoSuggestClient
    {
        private readonly string apiKey;
        private readonly HttpClient client;

        private static HttpClient DefaultClient = new HttpClient();

        public GeoSuggestClient(string apiKey)
        {
            this.apiKey = apiKey;
            client = DefaultClient;
        }

        public GeoSuggestClient(HttpClient client, string apiKey)
        {
            if (client == null) throw new ArgumentNullException("client");
            this.client = client;
        }
        private static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public async Task<GeoSuggestResponse> GeoSuggest(GeoSuggestRequest request,
            CancellationToken cancellationToken = default)
        {
            var collection = new System.Collections.Specialized.NameValueCollection
            {
                ["apikey"] = apiKey
            };
            request.Fill(collection);
            var isFirst = true;
            var resultUrl = "https://suggest-maps.yandex.ru/v1/suggest?";
            foreach (var key in collection.AllKeys)
            {
                if(!isFirst)
                    resultUrl += "&";
                resultUrl += key+"=" + HttpUtility.UrlEncode(collection[key]);
                isFirst = false;
            }
            var response = await client.GetAsync(resultUrl, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GeoSuggestResponse>(result, Settings);
        }
    }
}
