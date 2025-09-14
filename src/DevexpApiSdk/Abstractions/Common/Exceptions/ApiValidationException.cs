using System.Net;

namespace DevexpApiSdk.Common.Exceptions
{
    public class ApiValidationException : ApiException
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ApiValidationException(
            string message,
            IReadOnlyDictionary<string, string[]> errors,
            string responseBody = null
        )
            : base(HttpStatusCode.BadRequest, message, responseBody)
        {
            Errors = errors;
        }
    }
}
