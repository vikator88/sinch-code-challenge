using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseBody { get; }
        public string ApiMessage { get; }

        public ApiException(
            HttpStatusCode statusCode,
            string message,
            string responseBody = null,
            string apiMessage = null
        )
            : base(message)
        {
            StatusCode = statusCode;
            ResponseBody = responseBody;
            ApiMessage = apiMessage;
        }
    }
}
