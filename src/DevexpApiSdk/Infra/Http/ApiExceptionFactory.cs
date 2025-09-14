using System.Net;
using DevexpApiSdk.Common.Exceptions;

namespace DevexpApiSdk.Http
{
    internal static class ApiExceptionFactory
    {
        public static ApiException Create(HttpResponseMessage response, string rawBody)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new ApiValidationException(
                        "Validation error",
                        new Dictionary<string, string[]>(),
                        rawBody
                    );

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return new ApiAuthException("Authentication/authorization failed", rawBody);

                case HttpStatusCode.NotFound:
                    return new ApiNotFoundException("Resource not found", rawBody);
                default:
                    if ((int)response.StatusCode >= 500)
                        return new ApiServerException(response.StatusCode, rawBody);
                    return new ApiException(response.StatusCode, "Unexpected API error", rawBody);
            }
        }
    }
}
