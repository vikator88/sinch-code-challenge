using System.Net;

namespace DevexpApiSdk.Common
{
    internal class DevexpApiResponse
    {
        internal HttpStatusCode StatusCode { get; }
        internal string RawBody { get; }

        internal DevexpApiResponse(HttpStatusCode statusCode, string rawBody = null)
        {
            StatusCode = statusCode;
            RawBody = rawBody ?? string.Empty;
        }
    }

    internal class DevexpApiResponse<T> : DevexpApiResponse
    {
        internal T Data { get; }

        internal DevexpApiResponse(T data, HttpStatusCode statusCode, string rawBody = null)
            : base(statusCode, rawBody)
        {
            Data = data;
        }
    }
}
