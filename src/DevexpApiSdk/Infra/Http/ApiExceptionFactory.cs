using System.Net;
using DevexpApiSdk.Abstractions.Common.ApiResponseDtos;
using DevexpApiSdk.Common.Exceptions;
using DevexpApiSdk.Infra.Utils;

namespace DevexpApiSdk.Http
{
    internal static class ApiExceptionFactory
    {
        internal static ApiException Create(HttpResponseMessage response, string rawBody)
        {
            ErrorResponseDto errorDto = JsonErrorResponseParser.Parse(rawBody);

            if (errorDto == null)
                return new ApiException(
                    response.StatusCode,
                    "API error",
                    rawBody,
                    "no additional error information"
                );

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new ApiValidationException(
                        "Validation error",
                        rawBody,
                        errorDto?.Message ?? "unknown validation error"
                    );

                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return new ApiAuthException(
                        "Authentication/authorization failed",
                        rawBody,
                        errorDto?.Message ?? "unauthorized"
                    );

                case HttpStatusCode.NotFound:
                    return new ApiNotFoundException(
                        "Resource not found",
                        rawBody,
                        errorDto?.Id ?? "unknown",
                        errorDto?.Message ?? "resource not found"
                    );
                default:
                    if ((int)response.StatusCode >= 500)
                        return new ApiServerException(
                            response.StatusCode,
                            rawBody,
                            errorDto?.Message ?? "unknown server error"
                        );
                    return new ApiException(
                        response.StatusCode,
                        "Unexpected API error",
                        rawBody,
                        errorDto?.Message ?? "unexpected error"
                    );
            }
        }
    }
}
