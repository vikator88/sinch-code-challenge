using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a validation error occurs (HTTP 400 Bad Request).
    /// </summary>
    public class ApiValidationException : ApiException
    {
        public ApiValidationException(
            string message,
            string responseBody = null,
            string apiMessage = null
        )
            : base(HttpStatusCode.BadRequest, message, responseBody, apiMessage) { }
    }
}
