using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a server error occurs (HTTP 5xx).
    /// </summary>
    public class ApiServerException : ApiException
    {
        public ApiServerException(
            HttpStatusCode statusCode,
            string responseBody = null,
            string apiMessage = null
        )
            : base(
                statusCode,
                $"Server error: {(int)statusCode} {statusCode}",
                responseBody,
                apiMessage
            ) { }
    }
}
