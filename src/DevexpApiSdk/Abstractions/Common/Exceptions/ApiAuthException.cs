using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when an API authentication error occurs (HTTP 401 Unauthorized or 403 Forbidden).
    /// </summary>
    public class ApiAuthException : ApiException
    {
        public ApiAuthException(
            string message,
            string responseBody = null,
            string apiMessage = null
        )
            : base(HttpStatusCode.Unauthorized, message, responseBody, apiMessage) { }
    }
}
