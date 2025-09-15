using System.Text.Json;
using DevexpApiSdk.Common;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Tests.Common
{
    [TestFixture]
    public class ApiOptionsBuilderTests
    {
        [Test]
        public void Build_DefaultOptions_ShouldMatchExpectedDefaults()
        {
            var options = DevexpApiOptionsBuilder.CreateDefault().Build();

            Assert.That(options.ApiKey, Is.EqualTo(string.Empty));
            Assert.That(options.Timeout, Is.EqualTo(TimeSpan.FromSeconds(30)));
            Assert.That(options.EnableAutoRetries, Is.True);
            Assert.That(options.MaxRetryAttempts, Is.EqualTo(3));
            Assert.That(
                options.RetryDelayInMilliseconds,
                Is.EqualTo(TimeSpan.FromMilliseconds(2000))
            );
            Assert.That(options.EnableBulkOperations, Is.False);
            Assert.That(options.MaxDegreeOfParallelism, Is.EqualTo(4));
            Assert.That(options.EnableLogging, Is.False);
            Assert.That(options.Logger, Is.Null);
            Assert.That(
                options.JsonOptions.PropertyNamingPolicy,
                Is.EqualTo(JsonNamingPolicy.CamelCase)
            );
            Assert.That(options.DefaultPageSize, Is.EqualTo(20));
        }

        [Test]
        public void WithApiKey_ShouldSetApiKey()
        {
            var options = DevexpApiOptionsBuilder.CreateDefault().WithApiKey("myKey").Build();

            Assert.That(options.ApiKey, Is.EqualTo("myKey"));
        }

        [Test]
        public void WithTimeout_ShouldSetTimeout()
        {
            var timeout = TimeSpan.FromSeconds(10);

            var options = DevexpApiOptionsBuilder.CreateDefault().WithTimeout(timeout).Build();

            Assert.That(options.Timeout, Is.EqualTo(timeout));
        }

        [Test]
        public void EnableRetries_ShouldEnableRetriesAndSetValues()
        {
            var delay = TimeSpan.FromSeconds(5);

            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .EnableRetries(maxAttempts: 5, delay: delay)
                .Build();

            Assert.That(options.EnableAutoRetries, Is.True);
            Assert.That(options.MaxRetryAttempts, Is.EqualTo(5));
            Assert.That(options.RetryDelayInMilliseconds, Is.EqualTo(delay));
        }

        [Test]
        public void EnableBulkOperations_ShouldEnableBulkOperationsAndSetDegree()
        {
            var options = DevexpApiOptionsBuilder.CreateDefault().EnableBulkOperations(8).Build();

            Assert.That(options.EnableBulkOperations, Is.True);
            Assert.That(options.MaxDegreeOfParallelism, Is.EqualTo(8));
        }

        [Test]
        public void EnableLogging_ShouldEnableLoggingAndAssignLogger()
        {
            var fakeLogger = new FakeLogger();

            var options = DevexpApiOptionsBuilder.CreateDefault().EnableLogging(fakeLogger).Build();

            Assert.That(options.EnableLogging, Is.True);
            Assert.That(options.Logger, Is.EqualTo(fakeLogger));
        }

        [Test]
        public void WithJsonOptions_ShouldReplaceJsonOptions()
        {
            var customOptions = new JsonSerializerOptions { PropertyNamingPolicy = null };

            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithJsonOptions(customOptions)
                .Build();

            Assert.That(options.JsonOptions, Is.EqualTo(customOptions));
        }

        [Test]
        public void WithPageSize_ShouldSetDefaultPageSize()
        {
            var options = DevexpApiOptionsBuilder.CreateDefault().WithPageSize(50).Build();

            Assert.That(options.DefaultPageSize, Is.EqualTo(50));
        }

        [Test]
        public void Builder_ShouldBeImmutable_WhenChained()
        {
            var builder = DevexpApiOptionsBuilder.CreateDefault();

            var options1 = builder.WithApiKey("first").Build();
            var options2 = builder.WithApiKey("second").Build();

            Assert.That(options1.ApiKey, Is.EqualTo("first"));
            Assert.That(options2.ApiKey, Is.EqualTo("second"));
            Assert.That(options1, Is.Not.EqualTo(options2));
        }

        // Fake logger for testing logging options
        private class FakeLogger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state) => null!;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception exception,
                Func<TState, Exception, string> formatter
            ) { }
        }
    }
}
