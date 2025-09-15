using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DevexpApiSdk.Http
{
    internal static class RequestBuilder
    {
        public static HttpRequestMessage Build(HttpMethod method, string path, object body)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            var request = new HttpRequestMessage(method, path);

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body, jsonSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return request;
        }
    }
}
