using System.Text.Json;
using System.Text.Json.Serialization;
using DevexpApiSdk.Common.Metrics;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Common
{
    public record DevexpApiOptions
    {
        internal DevexpApiOptions() { }

        // Auth
        public string ApiKey { get; init; } = string.Empty;

        // HTTP
        public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);

        // Retries
        public bool EnableAutoRetries { get; init; } = true;
        public int MaxRetryAttempts { get; init; } = 3;
        public TimeSpan RetryDelayInMilliseconds { get; init; } = TimeSpan.FromMilliseconds(2000);

        // Bulk Ops
        public bool EnableBulkOperations { get; init; } = false;
        public int MaxDegreeOfParallelism { get; init; } = 4;

        // Logging
        public bool EnableLogging { get; init; } = false;
        public ILogger Logger { get; init; }

        // Metrics
        public Action<OperationPerformanceMetric> OnOperationCompleted { get; init; }

        // Serialization
        public JsonSerializerOptions JsonOptions { get; init; } =
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            };

        // Paging
        public int DefaultPageSize { get; init; } = 20;
    }
}
