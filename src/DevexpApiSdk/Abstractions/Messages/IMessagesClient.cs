using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Messages.Models;

namespace DevexpApiSdk.Messages
{
    public interface IMessagesClient
    {
        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<Message> GetMessageByIdAsync(Guid messageId);
        Task<Message> SendMessageAsync(string from, string messageContent, Guid toContactId);
        Task<Message> SendMessageAsync(string from, string messageContent, Contact toContact);
        Task<IEnumerable<Message>> SendMessageAsync(
            string from,
            string messageContent,
            Contact[] toContacts
        );
    }
}
