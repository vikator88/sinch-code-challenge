using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts;
using DevexpApiSdk.Http;
using DevexpApiSdk.Messages;

namespace DevexpApiSdk
{
    /// <summary>
    /// Provides a strongly-typed client wrapper for interacting with the Devexp API.
    /// </summary>
    /// <remarks>
    /// This client encapsulates HTTP communication with the Devexp API and exposes
    /// typed sub-clients for managing specific resource domains such as <see cref="Contacts"/>
    /// and <see cref="Messages"/>.
    /// </remarks>
    public sealed class DevexpApiClient : IDevexpApiClient
    {
        private readonly IDevexpApiHttpClient _http;
        private readonly DevexpApiOptions _options;

        /// <summary>
        /// Provides access to all Contacts API operations, such as creating, retrieving, updating,
        /// and deleting contacts.
        /// </summary>
        /// <remarks>
        /// This property returns an instance of <see cref="IContactsClient"/> that
        /// wraps the underlying HTTP communication with the Contacts resource domain.
        /// </remarks>
        public IContactsClient Contacts { get; }

        /// <summary>
        /// Provides access to all Messages API operations, including sending messages
        /// and retrieving delivery status.
        /// </summary>
        /// <remarks>
        /// This property returns an instance of <see cref="IMessagesClient"/> that
        /// encapsulates all messaging-related endpoints.
        /// </remarks>
        public IMessagesClient Messages { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevexpApiClient"/> class with the specified options.
        /// </summary>
        /// <param name="options">
        /// Optional configuration options for the API client. If <c>null</c>, a default configuration
        /// will be created using <see cref="DevexpApiOptionsBuilder.CreateDefault"/>.
        /// </param>
        /// <exception cref="ApiKeyMissingException">
        /// Thrown when the provided <paramref name="options"/> do not contain a valid API key.
        /// </exception>
        public DevexpApiClient(DevexpApiOptions options = null)
        {
            _options = options ?? DevexpApiOptionsBuilder.CreateDefault().Build();

            // HTTP client interno
            _http = new DefaultDevexpApiHttpClient(_options);

            // Subclientes
            Contacts = new ContactsClient(_http, _options);
            Messages = new MessagesClient(_http, _options);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DevexpApiClient"/> class
        /// using the specified API key.
        /// </summary>
        /// <param name="apiKey">
        /// The API key used for authenticating requests against the Devexp API.
        /// </param>
        /// <exception cref="ApiKeyMissingException">
        /// Thrown when <paramref name="apiKey"/> is <c>null</c>, empty, or invalid.
        /// </exception>
        public DevexpApiClient(string apiKey)
            : this(new DevexpApiOptionsBuilder().WithApiKey(apiKey).Build()) { }

        public void Dispose()
        {
            _http.Dispose();
        }
    }
}
