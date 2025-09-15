using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
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
