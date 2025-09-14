using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace DevexpApiSdk.Http
{
    internal static class RequestBuilder
    {
        public static HttpRequestMessage Build(
            HttpMethod method,
            string path,
            object body,
            JsonSerializerOptions jsonOptions
        )
        {
            var request = new HttpRequestMessage(method, path);

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body, jsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return request;
        }
    }
}
