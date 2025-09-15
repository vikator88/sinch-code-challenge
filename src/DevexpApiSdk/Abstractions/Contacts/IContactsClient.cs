using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Contacts
{
    public interface IContactsClient
    {
        Task<IPagedResult<Contact>> GetContactsAsync(
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken ct = default
        );
        Task<Contact> GetContactByIdAsync(Guid contactId, CancellationToken ct = default);
        Task<Contact> AddContactAsync(
            string contactName,
            string contactPhone,
            CancellationToken ct = default
        );
        Task<Contact> AddContactAsync(
            CreateContactRequest createContactRequest,
            CancellationToken ct = default
        );
        Task<IReadOnlyList<Contact>> AddContactsAsync(
            IEnumerable<CreateContactRequest> createContactRequests,
            CancellationToken ct = default
        );
        Task<Contact> UpdateContactAsync(
            Guid contactId,
            string contactName,
            string contactPhone,
            CancellationToken ct = default
        );
        Task<Contact> UpdateContactAsync(
            Contact updateContactRequest,
            CancellationToken ct = default
        );
        Task<IReadOnlyList<Contact>> UpdateContactsAsync(
            IEnumerable<Contact> updateContactRequests,
            CancellationToken ct = default
        );

        Task DeleteContactAsync(Guid contactId, CancellationToken ct = default);
        Task DeleteContactAsync(Contact contact, CancellationToken ct = default);
        Task DeleteContactsAsync(IEnumerable<Guid> contactIds, CancellationToken ct = default);
        Task DeleteContactsAsync(IEnumerable<Contact> contacts, CancellationToken ct = default);
    }
}
