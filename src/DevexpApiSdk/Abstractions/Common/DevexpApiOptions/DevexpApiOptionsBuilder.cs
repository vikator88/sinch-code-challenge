using System.Text.Json;
using DevexpApiSdk.Common.Metrics;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Common
{
    public class DevexpApiOptionsBuilder
    {
        private DevexpApiOptions _options = new DevexpApiOptions();

        public static DevexpApiOptionsBuilder CreateDefault()
        {
            return new DevexpApiOptionsBuilder();
        }

        public DevexpApiOptionsBuilder WithApiKey(string apiKey)
        {
            _options = _options with { ApiKey = apiKey };
            return this;
        }

        public DevexpApiOptionsBuilder WithTimeout(TimeSpan timeout)
        {
            _options = _options with { Timeout = timeout };
            return this;
        }

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

        public DevexpApiOptionsBuilder EnableBulkOperations(int maxDegreeOfParallelism = 4)
        {
            _options = _options with
            {
                EnableBulkOperations = true,
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
            return this;
        }

        public DevexpApiOptionsBuilder EnableLogging(ILogger logger)
        {
            _options = _options with { EnableLogging = true, Logger = logger };
            return this;
        }

        public DevexpApiOptionsBuilder WithOperationProfiler(
            Action<OperationPerformanceMetric> hook
        )
        {
            _options = _options with { OnOperationCompleted = hook };
            return this;
        }

        public DevexpApiOptionsBuilder WithJsonOptions(JsonSerializerOptions options)
        {
            _options = _options with { JsonOptions = options };
            return this;
        }

        public DevexpApiOptionsBuilder WithPageSize(int pageSize)
        {
            _options = _options with { DefaultPageSize = pageSize };
            return this;
        }

        public DevexpApiOptions Build()
        {
            return _options;
        }
    }
}
