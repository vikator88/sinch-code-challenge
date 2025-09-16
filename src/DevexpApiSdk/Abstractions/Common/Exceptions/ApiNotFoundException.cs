using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested resource is not found (HTTP 404 Not Found).
    /// </summary>
    public class ApiNotFoundException : ApiException
    {
        public string ResourceId { get; }

        public ApiNotFoundException(
            string resource,
            string responseBody = null,
            string resourceId = null,
            string apiMessage = null
        )
            : base(HttpStatusCode.NotFound, $"{resource} not found", responseBody, apiMessage)
        {
            ResourceId = resourceId;
        }
    }
}
