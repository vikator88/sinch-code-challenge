using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    public class ApiServerException : ApiException
    {
        public ApiServerException(HttpStatusCode statusCode, string responseBody = null)
            : base(statusCode, $"Server error: {(int)statusCode} {statusCode}", responseBody) { }
    }
}
