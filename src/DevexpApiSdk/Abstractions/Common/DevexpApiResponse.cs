using System.Net;

namespace DevexpApiSdk.Common
{
    public class DevexpApiResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string RawBody { get; }

        public DevexpApiResponse(HttpStatusCode statusCode, string rawBody = null)
        {
            StatusCode = statusCode;
            RawBody = rawBody ?? string.Empty;
        }
    }

    public class DevexpApiResponse<T> : DevexpApiResponse
    {
        public T Data { get; }

        public DevexpApiResponse(T data, HttpStatusCode statusCode, string rawBody = null)
            : base(statusCode, rawBody)
        {
            Data = data;
        }
    }
}
