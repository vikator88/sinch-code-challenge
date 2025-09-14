using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    public class ApiNotFoundException : ApiException
    {
        public ApiNotFoundException(string resource, string responseBody = null)
            : base(HttpStatusCode.NotFound, $"{resource} not found", responseBody) { }
    }
}
