using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Messages.Models;

namespace DevexpApiSdk.Messages
{
    public interface IMessagesClient
    {
        Task<IPagedResult<Message>> GetMessagesAsync(CancellationToken ct = default);
        Task<Message> GetMessageByIdAsync(Guid messageId, CancellationToken ct = default);
        Task<Message> SendMessageAsync(
            string from,
            string messageContent,
            Guid toContactId,
            CancellationToken ct = default
        );
        Task<Message> SendMessageAsync(
            string from,
            string messageContent,
            Contact toContact,
            CancellationToken ct = default
        );
        Task<Message> SendMessageAsync(
            CreateMessageRequest createMessageRequest,
            CancellationToken ct = default
        );
        Task<IReadOnlyList<Message>> SendMessageAsync(
            string from,
            string messageContent,
            Contact[] toContacts,
            CancellationToken ct = default
        );
    }
}
