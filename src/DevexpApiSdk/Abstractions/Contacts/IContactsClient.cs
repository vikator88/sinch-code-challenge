using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Contacts
{
    public interface IContactsClient
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int contactId);
        Task<Contact> AddContactAsync(string contactName, string contactPhone);
        Task<Contact> AddContactAsync(CreateContactRequest createContactRequest);
        Task<IEnumerable<Contact>> AddContactsAsync(
            IEnumerable<CreateContactRequest> createContactRequests
        );
        Task<Contact> UpdateContactAsync(int contactId, string contactName, string contactPhone);
        Task<Contact> UpdateContactAsync(Contact updateContactRequest);
        Task<IEnumerable<Contact>> UpdateContactsAsync(IEnumerable<Contact> updateContactRequests);
    }
}
