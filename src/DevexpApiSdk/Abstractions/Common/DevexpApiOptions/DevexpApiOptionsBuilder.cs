using DevexpApiSdk.Common.Exceptions;
using DevexpApiSdk.Common.Metrics;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Common
{
    /// <summary>
    /// Provides a fluent builder for creating <see cref="DevexpApiOptions"/> instances.
    /// </summary>
    /// <remarks>
    /// This builder follows a fluent interface pattern, allowing chaining of configuration methods
    /// before calling <see cref="Build"/> to produce an immutable <see cref="DevexpApiOptions"/> instance.
    ///
    /// The <see cref="ApiKey"/> property must be set using <see cref="WithApiKey"/> before invoking <see cref="Build"/>,
    /// otherwise an <see cref="Exceptions.ApiKeyMissingException"/> will be thrown.
    /// </remarks>
    public class DevexpApiOptionsBuilder
    {
        private DevexpApiOptions _options = new DevexpApiOptions();

        /// <summary>
        /// Creates a new <see cref="DevexpApiOptionsBuilder"/> instance with default values.
        /// </summary>
        /// <returns>A new <see cref="DevexpApiOptionsBuilder"/> instance.</returns>
        public static DevexpApiOptionsBuilder CreateDefault()
        {
            return new DevexpApiOptionsBuilder();
        }

        /// <summary>
        /// Sets the API key to be used for authenticating requests.
        /// </summary>
        /// <param name="apiKey">The API key string.</param>
        /// <returns>The current <see cref="DevexpApiOptionsBuilder"/> instance.</returns>
        public DevexpApiOptionsBuilder WithApiKey(string apiKey)
        {
            _options = _options with { ApiKey = apiKey };
            return this;
        }

        /// <summary>
        /// Sets the maximum HTTP timeout for requests.
        /// </summary>
        /// <param name="timeout">The maximum time to wait before a request times out.</param>
        /// <returns>The current <see cref="DevexpApiOptionsBuilder"/> instance.</returns>
        public DevexpApiOptionsBuilder WithTimeout(TimeSpan timeout)
        {
            _options = _options with { Timeout = timeout };
            return this;
        }

        /// <summary>
        /// Enables automatic retries for transient failures.
        /// </summary>
        /// <param name="maxAttempts">The maximum number of retry attempts. Defaults to 3.</param>
        /// <param name="delay">
        /// The delay interval between retries. Defaults to 2000 milliseconds if not provided.
        /// </param>
        /// <returns>The current <see cref="DevexpApiOptionsBuilder"/> instance.</returns>
        public DevexpApiOptionsBuilder EnableRetries(int maxAttempts = 3, TimeSpan? delay = null)
        {
            _options = _options with
            {
                EnableAutoRetries = true,
                MaxRetryAttempts = maxAttempts,
                RetryDelayInMilliseconds = delay ?? TimeSpan.FromMilliseconds(2000)
            };
            return this;
        }

        /// <summary>
        /// Enables logging and assigns the specified <see cref="ILogger"/> instance.
        /// </summary>
        /// <param name="logger">The logger instance used for diagnostic logging.</param>
        /// <returns>The current <see cref="DevexpApiOptionsBuilder"/> instance.</returns>
        public DevexpApiOptionsBuilder EnableLogging(ILogger logger)
        {
            _options = _options with { EnableLogging = true, Logger = logger };
            return this;
        }

        /// <summary>
        /// Enables operation profiling and sets a hook callback for performance metrics collection.
        /// </summary>
        /// <param name="hook">The callback invoked after each operation with performance data.</param>
        /// <returns>The current <see cref="DevexpApiOptionsBuilder"/> instance.</returns>
        public DevexpApiOptionsBuilder WithOperationProfiler(
            Action<OperationPerformanceMetric> hook
        )
        {
            _options = _options with { EnableMetrics = true };
            _options = _options with { OnOperationCompleted = hook };
            return this;
        }

        /// <summary>
        /// Sets the default page size for paginated API requests.
        /// </summary>
        /// <param name="pageSize">The default number of items per page.</param>
        /// <returns>The current <see cref="DevexpApiOptionsBuilder"/> instance.</returns>
        public DevexpApiOptionsBuilder WithPageSize(int pageSize)
        {
            _options = _options with { DefaultPageSize = pageSize };
            return this;
        }

        /// <summary>
        /// Builds and returns a new <see cref="DevexpApiOptions"/> instance with the configured values.
        /// </summary>
        /// <remarks>
        /// This method validates that an API key has been provided using <see cref="WithApiKey"/>.
        /// If the API key is not set, an <see cref="Exceptions.ApiKeyMissingException"/> will be thrown.
        /// </remarks>
        /// <returns>A fully configured <see cref="DevexpApiOptions"/> instance.</returns>
        /// <exception cref="Exceptions.ApiKeyMissingException">
        /// Thrown if no API key was provided.
        /// </exception>
        public DevexpApiOptions Build()
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                throw new ApiKeyMissingException();
            }
            return _options;
        }
    }
}
