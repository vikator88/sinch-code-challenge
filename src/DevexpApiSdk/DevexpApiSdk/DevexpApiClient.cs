using DevexpApiSdk.Common;
using DevexpApiSdk.Common.Metrics;
using DevexpApiSdk.Contacts;
using DevexpApiSdk.Http;
using DevexpApiSdk.Messages;

namespace DevexpApiSdk
{
    /// <summary>
    /// Default implementation of <see cref="IApiClient"/>.
    /// </summary>
    public class DevexpApiClient : IDevexpApiClient
    {
        private readonly IDevexpApiHttpClient _http;
        private readonly DevexpApiOptions _options;

        public IContactsClient Contacts { get; }
        public IMessagesClient Messages { get; }
        private const string baseUrl = "http://localhost:8080/";

        public DevexpApiClient(DevexpApiOptions options = null)
        {
            _options = options ?? DevexpApiOptionsBuilder.CreateDefault().Build();

            // HTTP client interno
            _http = new DefaultDevexpApiHttpClient(baseUrl, _options);

            if (_options.EnableMetrics)
            {
                Contacts = ProfilingProxyFactory.Create<IContactsClient>(
                    new ContactsClient(_http, _options),
                    _options
                );
                Messages = ProfilingProxyFactory.Create<IMessagesClient>(
                    new MessagesClient(_http, _options),
                    _options
                );
            }
            else
            {
                Contacts = new ContactsClient(_http, _options);
                Messages = new MessagesClient(_http, _options);
            }
        }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
