using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using DevexpApiSdk.Common;
using DevexpApiSdk.Common.DevexpApiSdk.Common;
using DevexpApiSdk.Common.Exceptions;
using Microsoft.Extensions.Logging;
using Polly;

namespace DevexpApiSdk.Http
{
    internal class DefaultApiHttpClient : IDevexpApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly DevexpApiOptions _options;
        private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy;

        public DefaultApiHttpClient(
            string baseUrl,
            DevexpApiOptions options,
            HttpClient httpClient = null
        )
        {
            _options = options;

            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient.Timeout = _options.Timeout;

            // Always attach bearer token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                _options.ApiKey
            );

            // Setup retry policy if enabled
            if (_options.EnableAutoRetries)
            {
                _retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .OrResult<HttpResponseMessage>(r =>
                        (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout
                    )
                    .WaitAndRetryAsync(
                        _options.MaxRetryAttempts,
                        attempt => _options.RetryDelayInMilliseconds,
                        (outcome, delay, attempt, ctx) =>
                        {
                            _options.Logger?.LogWarning(
                                "Retry {Attempt} after {Delay} due to {Reason}",
                                attempt,
                                delay,
                                outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()
                            );
                        }
                    );
            }
            else
            {
                _retryPolicy = Policy.NoOpAsync<HttpResponseMessage>();
            }
        }

        public async Task<DevexpApiResponse<T>> SendAsync<T>(
            HttpMethod method,
            string path,
            object body = null,
            CancellationToken ct = default
        )
        {
            var request = BuildRequest(method, path, body);

            // Log before request
            _options.Logger?.LogInformation("HTTP {Method} {Path}", method, path);

            var response = await _httpClient.SendAsync(request, ct);

            var rawBody = await response.Content.ReadAsStringAsync(ct);

            // Log after response
            _options.Logger?.LogInformation(
                "Response {StatusCode} {Path}",
                response.StatusCode,
                path
            );

            if (!response.IsSuccessStatusCode)
            {
                ThrowApiException(response, rawBody);
            }

            T data = default;
            if (!string.IsNullOrWhiteSpace(rawBody))
            {
                data = JsonSerializer.Deserialize<T>(rawBody, _options.JsonOptions);
            }

            return new DevexpApiResponse<T>(data, response.StatusCode, rawBody);
        }

        public async Task<DevexpApiResponse> SendAsync(
            HttpMethod method,
            string path,
            object body = null,
            CancellationToken ct = default
        )
        {
            var request = BuildRequest(method, path, body);

            // Log before request
            _options.Logger?.LogInformation("HTTP {Method} {Path}", method, path);

            var response = await _httpClient.SendAsync(request, ct);
            var rawBody = await response.Content.ReadAsStringAsync(ct);

            // Log after response
            _options.Logger?.LogInformation(
                "Response {StatusCode} {Path}",
                response.StatusCode,
                path
            );

            if (!response.IsSuccessStatusCode)
            {
                ThrowApiException(response, rawBody);
            }

            return new DevexpApiResponse(response.StatusCode, rawBody);
        }

        private HttpRequestMessage BuildRequest(HttpMethod method, string path, object body)
        {
            var request = new HttpRequestMessage(method, path);

            if (body != null)
            {
                var json = JsonSerializer.Serialize(body, _options.JsonOptions);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return request;
        }

        private static void ThrowApiException(HttpResponseMessage response, string rawBody)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ApiValidationException(
                        "Validation error",
                        new Dictionary<string, string[]>(),
                        rawBody
                    );
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    throw new ApiAuthException("Authentication/authorization failed", rawBody);
                case HttpStatusCode.NotFound:
                    throw new ApiNotFoundException("Resource not found", rawBody);
                default:
                    if ((int)response.StatusCode >= 500)
                        throw new ApiServerException(response.StatusCode, rawBody);

                    throw new ApiException(response.StatusCode, "Unexpected API error", rawBody);
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
