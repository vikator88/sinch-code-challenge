using DevexpApiSdk.Common;
using DevexpApiSdk.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Tests.Common
{
    [TestFixture]
    public class ApiOptionsBuilderTests
    {
        [Test]
        public void Build_DefaultOptions_ShouldMatchExpectedDefaults()
        {
            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("api-key") // To avoid exception on missing API key
                .Build();

            Assert.That(options.Timeout, Is.EqualTo(TimeSpan.FromSeconds(30)));
            Assert.That(options.EnableAutoRetries, Is.True);
            Assert.That(options.MaxRetryAttempts, Is.EqualTo(3));
            Assert.That(
                options.RetryDelayInMilliseconds,
                Is.EqualTo(TimeSpan.FromMilliseconds(2000))
            );
            Assert.That(options.EnableLogging, Is.False);
            Assert.That(options.Logger, Is.Null);

            Assert.That(options.DefaultPageSize, Is.EqualTo(20));
        }

        [Test]
        public void WithApiKey_ShouldSetApiKey()
        {
            var options = DevexpApiOptionsBuilder.CreateDefault().WithApiKey("myKey").Build();

            Assert.That(options.ApiKey, Is.EqualTo("myKey"));
        }

        [Test]
        public void Build_WithoutApiKey_ShouldThrowException()
        {
            var builder = DevexpApiOptionsBuilder.CreateDefault();
            // Not setting API key to simulate missing key

            var ex = Assert.Throws<ApiKeyMissingException>(() => builder.Build());
            Assert.That(ex.Message, Contains.Substring("No API key was provided."));
        }

        [Test]
        public void WithTimeout_ShouldSetTimeout()
        {
            var timeout = TimeSpan.FromSeconds(10);

            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("api-key") // To avoid exception on missing API key
                .WithTimeout(timeout)
                .Build();

            Assert.That(options.Timeout, Is.EqualTo(timeout));
        }

        [Test]
        public void EnableRetries_ShouldEnableRetriesAndSetValues()
        {
            var delay = TimeSpan.FromSeconds(5);

            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("api-key") // To avoid exception on missing API key
                .EnableRetries(maxAttempts: 5, delay: delay)
                .Build();

            Assert.That(options.EnableAutoRetries, Is.True);
            Assert.That(options.MaxRetryAttempts, Is.EqualTo(5));
            Assert.That(options.RetryDelayInMilliseconds, Is.EqualTo(delay));
        }

        [Test]
        public void EnableLogging_ShouldEnableLoggingAndAssignLogger()
        {
            var fakeLogger = new FakeLogger();

            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("api-key") // To avoid exception on missing API key
                .EnableLogging(fakeLogger)
                .Build();

            Assert.That(options.EnableLogging, Is.True);
            Assert.That(options.Logger, Is.EqualTo(fakeLogger));
        }

        [Test]
        public void WithPageSize_ShouldSetDefaultPageSize()
        {
            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("api-key") // To avoid exception on missing API key
                .WithPageSize(50)
                .Build();

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
