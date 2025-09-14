using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    public class ApiAuthException : ApiException
    {
        public ApiAuthException(string message, string responseBody = null)
            : base(HttpStatusCode.Unauthorized, message, responseBody) { }
    }
}
