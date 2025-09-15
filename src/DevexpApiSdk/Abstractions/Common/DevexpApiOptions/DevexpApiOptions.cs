using DevexpApiSdk.Common.Metrics;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Common
{
    /// <summary>
    /// Represents configuration options for the Devexp API client.
    /// </summary>
    /// <remarks>
    /// This record encapsulates all runtime configuration parameters used by the
    /// <see cref="DevexpApiClient"/> and its sub-clients. Options include base URL,
    /// authentication, HTTP behavior, retry policies, logging, and metrics collection.
    ///
    /// Instances of this class are created via the
    /// <see cref="DevexpApiOptionsBuilder"/> to ensure correct initialization.
    /// </remarks>
    public record DevexpApiOptions
    {
        internal DevexpApiOptions() { }

        // Base URL

        /// <summary>
        /// Gets the base URL of the Devexp API.
        /// </summary>
        /// <remarks>
        /// Defaults to <c>http://localhost:3000</c>. In production scenarios,
        /// this should point to the official API endpoint.
        /// </remarks>
        public string BaseUrl { get; init; } = "http://localhost:3000";

        // Auth

        /// <summary>
        /// Gets the API key used for authenticating requests.
        /// </summary>
        /// <remarks>
        /// If not set, the <see cref="DevexpApiOptionsBuilder.Build"/> method will throw
        /// <see cref="Common.Exceptions.ApiKeyMissingException"/>.
        /// </remarks>
        public string ApiKey { get; init; } = string.Empty;

        // HTTP

        /// <summary>
        /// Gets the maximum duration to wait for an HTTP response before timing out.
        /// </summary>
        /// <remarks>Defaults to 30 seconds.</remarks>
        public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);

        // Retries

        /// <summary>
        /// Gets a value indicating whether automatic retries should be enabled
        /// for transient failures.
        /// </summary>
        /// <remarks>Defaults to <c>true</c>.</remarks>
        public bool EnableAutoRetries { get; init; } = true;

        /// <summary>
        /// Gets the maximum number of retry attempts when <see cref="EnableAutoRetries"/> is enabled.
        /// </summary>
        /// <remarks>Defaults to <c>3</c>.</remarks>
        public int MaxRetryAttempts { get; init; } = 3;

        /// <summary>
        /// Gets the delay interval between retry attempts.
        /// </summary>
        /// <remarks>Defaults to 2000 milliseconds.</remarks>
        public TimeSpan RetryDelayInMilliseconds { get; init; } = TimeSpan.FromMilliseconds(2000);

        // Logging

        /// <summary>
        /// Gets a value indicating whether request and response logging is enabled.
        /// </summary>
        /// <remarks>
        /// Logging is performed through the <see cref="Logger"/> instance
        /// if provided. Defaults to <c>false</c>.
        /// </remarks>
        public bool EnableLogging { get; init; } = false;

        /// <summary>
        /// Gets the logger instance used for diagnostic logging.
        /// </summary>
        /// <remarks>
        /// Must implement <see cref="ILogger"/> from Microsoft.Extensions.Logging.
        /// </remarks>
        public ILogger Logger { get; init; }

        // Metrics

        /// <summary>
        /// Gets a value indicating whether performance metrics collection is enabled.
        /// </summary>
        /// <remarks>
        /// When enabled, the <see cref="OnOperationCompleted"/> callback will be
        /// invoked after each API operation with performance data.
        /// </remarks>
        public bool EnableMetrics { get; init; } = false;

        /// <summary>
        /// Gets the callback invoked when an API operation completes,
        /// providing performance metrics data.
        /// </summary>
        public Action<OperationPerformanceMetric> OnOperationCompleted { get; init; }

        // Paging

        /// <summary>
        /// Gets the default page size used in paginated API requests.
        /// </summary>
        /// <remarks>Defaults to <c>20</c>.</remarks>
        public int DefaultPageSize { get; init; } = 20;
    }
}
