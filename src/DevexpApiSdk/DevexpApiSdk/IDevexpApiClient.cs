using DevexpApiSdk.Contacts;
using DevexpApiSdk.Messages;

namespace DevexpApiSdk
{
    /// <summary>
    /// Root entry point for interacting with the Devexp API.
    /// Provides access to sub-clients (Contacts, Messages).
    /// </summary>
    public interface IDevexpApiClient : IDisposable
    {
        IContactsClient Contacts { get; }
        IMessagesClient Messages { get; }
    }
}
