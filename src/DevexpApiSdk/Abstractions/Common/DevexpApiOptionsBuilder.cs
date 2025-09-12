using System.Text.Json;
using DevexpApiSdk.Common.DevexpApiSdk.Common;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Common
{
    public class ApiOptionsBuilder
    {
        private DevexpApiOptions _options = new DevexpApiOptions();

        public static ApiOptionsBuilder CreateDefault()
        {
            return new ApiOptionsBuilder();
        }

        public ApiOptionsBuilder WithApiKey(string apiKey)
        {
            _options = _options with { ApiKey = apiKey };
            return this;
        }

        public ApiOptionsBuilder WithTimeout(TimeSpan timeout)
        {
            _options = _options with { Timeout = timeout };
            return this;
        }

        public ApiOptionsBuilder EnableRetries(int maxAttempts = 3, TimeSpan? delay = null)
        {
            _options = _options with
            {
                EnableAutoRetries = true,
                MaxRetryAttempts = maxAttempts,
                RetryDelayInMilliseconds = delay ?? TimeSpan.FromMilliseconds(2000)
            };
            return this;
        }

        public ApiOptionsBuilder EnableBulkOperations(int maxDegreeOfParallelism = 4)
        {
            _options = _options with
            {
                EnableBulkOperations = true,
                MaxDegreeOfParallelism = maxDegreeOfParallelism
            };
            return this;
        }

        public ApiOptionsBuilder EnableLogging(ILogger logger)
        {
            _options = _options with { EnableLogging = true, Logger = logger };
            return this;
        }

        public ApiOptionsBuilder WithJsonOptions(JsonSerializerOptions options)
        {
            _options = _options with { JsonOptions = options };
            return this;
        }

        public ApiOptionsBuilder WithPageSize(int pageSize)
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
