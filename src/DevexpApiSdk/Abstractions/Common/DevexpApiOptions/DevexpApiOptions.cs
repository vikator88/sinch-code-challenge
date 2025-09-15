using DevexpApiSdk.Common.Metrics;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Common
{
    public record DevexpApiOptions
    {
        internal DevexpApiOptions() { }

        // Base URL
        public string BaseUrl { get; init; } = "http://localhost:3000";

        // Auth
        public string ApiKey { get; init; } = string.Empty;

        // HTTP
        public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);

        // Retries
        public bool EnableAutoRetries { get; init; } = true;
        public int MaxRetryAttempts { get; init; } = 3;
        public TimeSpan RetryDelayInMilliseconds { get; init; } = TimeSpan.FromMilliseconds(2000);

        // Logging
        public bool EnableLogging { get; init; } = false;
        public ILogger Logger { get; init; }

        // Metrics
        public bool EnableMetrics { get; init; } = false;
        public Action<OperationPerformanceMetric> OnOperationCompleted { get; init; }

        // Paging
        public int DefaultPageSize { get; init; } = 20;
    }
}
