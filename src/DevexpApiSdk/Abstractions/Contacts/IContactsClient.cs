using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Contacts
{
    /// <summary>
    /// Provides a strongly-typed client for interacting with the Contacts resource domain
    /// of the Devexp API.
    /// </summary>
    /// <remarks>
    /// This client encapsulates HTTP operations for creating, retrieving, updating,
    /// and deleting contact records. It performs input validation (e.g., E.164 phone numbers)
    /// and wraps execution with optional metrics <see cref="OperationPerformanceMetric"/>.
    /// </remarks>
    public interface IContactsClient
    {
        /// <summary>
        /// Retrieves a paged list of contacts.
        /// </summary>
        /// <param name="pageNumber">The index of the page to retrieve (1-based).</param>
        /// <param name="pageSize">The maximum number of contacts per page.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>A paged result containing the requested contacts.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<IPagedResult<Contact>> GetContactsAsync(
            int pageNumber,
            int pageSize,
            CancellationToken ct = default
        );

        /// <summary>
        /// Retrieves a paged list of contacts using the default page size.
        /// </summary>
        /// <param name="pageNumber">The index of the page to retrieve (1-based).</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>A paged result containing the requested contacts.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<IPagedResult<Contact>> GetContactsAsync(
            int pageNumber = 1,
            CancellationToken ct = default
        );

        /// <summary>
        /// Retrieves a single contact by its unique identifier.
        /// </summary>
        /// <param name="contactId">The unique identifier of the contact.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The requested <see cref="Contact"/> if found.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Contact> GetContactByIdAsync(Guid contactId, CancellationToken ct = default);

        /// <summary>
        /// Creates a new contact asynchronously with the specified name and phone number.
        /// </summary>
        /// <param name="name">The contact's display name.</param>
        /// <param name="phone">The contact's phone number in E.164 format.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The newly created <see cref="Contact"/>.</returns>
        /// <exception cref="InvalidPhoneNumberException">Thrown if the phone number is not in valid E.164 format.</exception>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiValidationException">Thrown if the request data is invalid.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Contact> AddContactAsync(
            string contactName,
            string contactPhone,
            CancellationToken ct = default
        );

        /// <summary>
        /// Creates a new contact asynchronously from a request model.
        /// </summary>
        /// <param name="createContactRequest">The request model containing the contact name and phone number.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The newly created <see cref="Contact"/>.</returns>
        /// <exception cref="InvalidPhoneNumberException">Thrown if the phone number is not in valid E.164 format.</exception>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiValidationException">Thrown if the request data is invalid.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Contact> AddContactAsync(
            CreateContactRequest createContactRequest,
            CancellationToken ct = default
        );

        /// <summary>
        /// Creates multiple contacts asynchronously.
        /// </summary>
        /// <param name="contacts">A collection of contact creation requests.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>A read-only list of newly created <see cref="Contact"/> records.</returns>
        /// <exception cref="InvalidPhoneNumberException">Thrown if the phone number is not in valid E.164 format.</exception>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiValidationException">Thrown if the request data is invalid.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<IReadOnlyList<Contact>> AddContactsAsync(
            IEnumerable<CreateContactRequest> createContactRequests,
            CancellationToken ct = default
        );

        /// <summary>
        /// Updates the specified contact with a new name and phone number.
        /// </summary>
        /// <param name="id">The unique identifier of the contact.</param>
        /// <param name="name">The new display name of the contact.</param>
        /// <param name="phone">The new phone number of the contact in E.164 format.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The updated <see cref="Contact"/>.</returns>
        /// <exception cref="InvalidPhoneNumberException">Thrown if the phone number is not in valid E.164 format.</exception>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Contact> UpdateContactAsync(
            Guid id,
            string name,
            string phone,
            CancellationToken ct = default
        );

        /// <summary>
        /// Updates an existing contact asynchronously.
        /// </summary>
        /// <param name="updateContactRequest">The contact entity containing the updated values.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The updated <see cref="Contact"/>.</returns>
        /// <exception cref="InvalidPhoneNumberException">Thrown if the phone number is not in valid E.164 format.</exception>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Contact> UpdateContactAsync(
            Contact updateContactRequest,
            CancellationToken ct = default
        );

        /// <summary>
        /// Updates multiple contacts asynchronously.
        /// </summary>
        /// <param name="updateContactRequests">A collection of contact entities with updated values.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>A read-only list of updated <see cref="Contact"/> records.</returns>
        /// <exception cref="InvalidPhoneNumberException">Thrown if the phone number is not in valid E.164 format.</exception>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<IReadOnlyList<Contact>> UpdateContactsAsync(
            IEnumerable<Contact> updateContactRequests,
            CancellationToken ct = default
        );

        /// <summary>
        /// Deletes a contact by its unique identifier.
        /// </summary>
        /// <param name="contactId">The unique identifier of the contact.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task DeleteContactAsync(Guid contactId, CancellationToken ct = default);

        /// <summary>
        /// Deletes the specified contact.
        /// </summary>
        /// <param name="contact">The contact to delete.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task DeleteContactAsync(Contact contact, CancellationToken ct = default);

        /// <summary>
        /// Deletes multiple contacts by their unique identifiers.
        /// </summary>
        /// <param name="contactIds">A collection of contact IDs.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task DeleteContactsAsync(IEnumerable<Guid> contactIds, CancellationToken ct = default);

        /// <summary>
        /// Deletes multiple contacts.
        /// </summary>
        /// <param name="contacts">A collection of contacts to delete.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task DeleteContactsAsync(IEnumerable<Contact> contacts, CancellationToken ct = default);
    }
}
