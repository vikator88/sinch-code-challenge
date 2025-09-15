using DevexpApiSdk.Contacts;
using DevexpApiSdk.Messages;

namespace DevexpApiSdk
{
    /// <summary>
    /// Defines the contract for the Devexp API client,
    /// providing access to all resource domain sub-clients.
    /// </summary>
    public interface IDevexpApiClient : IDisposable
    {
        /// <summary>
        /// Gets the client for interacting with the Contacts resource domain.
        /// </summary>
        IContactsClient Contacts { get; }

        /// <summary>
        /// Gets the client for interacting with the Messages resource domain.
        /// </summary>
        IMessagesClient Messages { get; }
    }
}
