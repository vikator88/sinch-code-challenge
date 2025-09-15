using System.Net;
using System.Net.Http.Headers;
using DevexpApiSdk.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;

namespace DevexpApiSdk.Http
{
    internal class DefaultDevexpApiHttpClient : IDevexpApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly DevexpApiOptions _options;
        private readonly AsyncPolicy<HttpResponseMessage> _retryPolicy;

        private readonly JsonSerializerSettings _jsonSettings;

        public DefaultDevexpApiHttpClient(string baseUrl, DevexpApiOptions options = null)
        {
            _options = options ?? DevexpApiOptionsBuilder.CreateDefault().Build();

            _httpClient = new HttpClient();
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

            _jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task<DevexpApiResponse<T>> SendAsync<T>(
            HttpMethod method,
            string path,
            object body = null,
            CancellationToken ct = default
        )
        {
            var request = RequestBuilder.Build(method, path, body);

            // Log before request
            _options.Logger?.LogInformation("HTTP {Method} {Path}", method, path);

            // Send request with retry policy
            var response = await _retryPolicy.ExecuteAsync(
                async ct2 =>
                {
                    return await _httpClient.SendAsync(request, ct2);
                },
                ct
            );

            var rawBody = await response.Content.ReadAsStringAsync(ct);

            // Log after response
            _options.Logger?.LogInformation(
                "Response {StatusCode} {Path}",
                response.StatusCode,
                path
            );

            if (!response.IsSuccessStatusCode)
            {
                throw ApiExceptionFactory.Create(response, rawBody);
            }

            T data = default;
            if (!string.IsNullOrWhiteSpace(rawBody))
            {
                data = JsonConvert.DeserializeObject<T>(rawBody, _jsonSettings);
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
            var request = RequestBuilder.Build(method, path, body);

            // Log before request
            _options.Logger?.LogInformation("HTTP {Method} {Path}", method, path);

            // Send request with retry policy
            var response = await _retryPolicy.ExecuteAsync(
                async ct2 =>
                {
                    return await _httpClient.SendAsync(request, ct2);
                },
                ct
            );
            var rawBody = await response.Content.ReadAsStringAsync(ct);

            // Log after response
            _options.Logger?.LogInformation(
                "Response {StatusCode} {Path}",
                response.StatusCode,
                path
            );

            if (!response.IsSuccessStatusCode)
            {
                throw ApiExceptionFactory.Create(response, rawBody);
            }

            return new DevexpApiResponse(response.StatusCode, rawBody);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
