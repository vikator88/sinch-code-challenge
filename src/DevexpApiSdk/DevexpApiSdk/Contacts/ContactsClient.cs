using System.Collections.Concurrent;
using DevexpApiSdk.Common;
using DevexpApiSdk.Common.Exceptions;
using DevexpApiSdk.Common.Metrics;
using DevexpApiSdk.Common.Validation;
using DevexpApiSdk.Contacts.ApiResponseDtos;
using DevexpApiSdk.Contacts.Mappers;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Http;

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
    internal class ContactsClient : IContactsClient
    {
        private readonly IDevexpApiHttpClient _http;
        private readonly string _resourcePath = "/contacts";
        private readonly DevexpApiOptions _options;

        internal ContactsClient(IDevexpApiHttpClient http, DevexpApiOptions options)
        {
            _http = http;
            _options = options;
        }

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
        public async Task<Contact> AddContactAsync(
            CreateContactRequest createContactRequest,
            CancellationToken ct = default
        )
        {
            return await AddContactAsync(createContactRequest.Name, createContactRequest.Phone, ct);
        }

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
        public async Task<Contact> AddContactAsync(
            string name,
            string phone,
            CancellationToken ct = default
        )
        {
            // Check if phone is E.164 format
            if (!PhoneNumberValidator.IsValidE164(phone))
                throw new InvalidPhoneNumberException(phone);

            // Wrap the operation with metrics collection
            return await OperationExecutor.ExecuteAsync<Contact>(
                "Contacts.AddContact",
                async () =>
                {
                    var body = new { name, phone };
                    var response = await _http.SendAsync<Contact>(
                        HttpMethod.Post,
                        _resourcePath,
                        body,
                        ct
                    );

                    return response.Data!;
                },
                _options
            );
        }

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
        public async Task<IReadOnlyList<Contact>> AddContactsAsync(
            IEnumerable<CreateContactRequest> contacts,
            CancellationToken ct = default
        )
        {
            // Wrap the operation with metrics collection
            // Added only in some methods to show how it can be done
            return await OperationExecutor.ExecuteAsync<IReadOnlyList<Contact>>(
                "Contacts.BulkAddContacts",
                async () =>
                {
                    var results = new List<Contact>();
                    foreach (var c in contacts)
                    {
                        results.Add(await AddContactAsync(c.Name, c.Phone, ct));
                    }
                    return results;
                },
                _options
            );
        }

        /// <summary>
        /// Deletes a contact by its unique identifier.
        /// </summary>
        /// <param name="contactId">The unique identifier of the contact.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        public async Task DeleteContactAsync(Guid contactId, CancellationToken ct = default)
        {
            await _http.SendAsync(HttpMethod.Delete, $"{_resourcePath}/{contactId}", null, ct);
        }

        /// <summary>
        /// Deletes the specified contact.
        /// </summary>
        /// <param name="contact">The contact to delete.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        public async Task DeleteContactAsync(Contact contact, CancellationToken ct = default)
        {
            await DeleteContactAsync(contact.Id, ct);
        }

        /// <summary>
        /// Deletes multiple contacts by their unique identifiers.
        /// </summary>
        /// <param name="contactIds">A collection of contact IDs.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        public async Task DeleteContactsAsync(
            IEnumerable<Guid> contactIds,
            CancellationToken ct = default
        )
        {
            foreach (var c in contactIds)
            {
                await DeleteContactAsync(c, ct);
            }
        }

        /// <summary>
        /// Deletes multiple contacts.
        /// </summary>
        /// <param name="contacts">A collection of contacts to delete.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        public Task DeleteContactsAsync(
            IEnumerable<Contact> contacts,
            CancellationToken ct = default
        )
        {
            return DeleteContactsAsync(contacts.Select(c => c.Id), ct);
        }

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
        public async Task<IPagedResult<Contact>> GetContactsAsync(
            int pageNumber = 1,
            int pageSize = 20,
            CancellationToken ct = default
        )
        {
            var response = await _http.SendAsync<ListContactsResponseDto>(
                HttpMethod.Get,
                $"{_resourcePath}?pageIndex={pageNumber}&max={pageSize}",
                null,
                ct
            );

            return ListContactsResponseMapper.MapToPagedResult(response.Data!);
        }

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
        public async Task<Contact> GetContactByIdAsync(
            Guid contactId,
            CancellationToken ct = default
        )
        {
            var response = await _http.SendAsync<Contact>(
                HttpMethod.Get,
                $"{_resourcePath}/{contactId}",
                ct
            );

            return response.Data;
        }

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
        public async Task<Contact> UpdateContactAsync(
            Guid id,
            string name,
            string phone,
            CancellationToken ct = default
        )
        {
            // Check if phone is E.164 format
            if (!PhoneNumberValidator.IsValidE164(phone))
                throw new InvalidPhoneNumberException(phone);
            var body = new { name, phone };
            var response = await _http.SendAsync<Contact>(
                HttpMethod.Patch,
                $"{_resourcePath}/{id}",
                body,
                ct
            );

            return response.Data;
        }

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
        public async Task<Contact> UpdateContactAsync(
            Contact updateContactRequest,
            CancellationToken ct = default
        )
        {
            return await UpdateContactAsync(
                updateContactRequest.Id,
                updateContactRequest.Name,
                updateContactRequest.Phone,
                ct
            );
        }

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
        public async Task<IReadOnlyList<Contact>> UpdateContactsAsync(
            IEnumerable<Contact> updateContactRequests,
            CancellationToken ct = default
        )
        {
            var results = new List<Contact>();
            foreach (var c in updateContactRequests)
            {
                results.Add(await UpdateContactAsync(c, ct));
            }
            return results;
        }
    }
}
