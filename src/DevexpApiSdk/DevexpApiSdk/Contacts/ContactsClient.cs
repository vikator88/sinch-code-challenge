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
    /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<Contact> AddContactAsync(
            CreateContactRequest createContactRequest,
            CancellationToken ct = default
        )
        {
            return await AddContactAsync(createContactRequest.Name, createContactRequest.Phone, ct);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task DeleteContactAsync(Guid contactId, CancellationToken ct = default)
        {
            await _http.SendAsync(HttpMethod.Delete, $"{_resourcePath}/{contactId}", null, ct);
        }

        /// <inheritdoc/>
        public async Task DeleteContactAsync(Contact contact, CancellationToken ct = default)
        {
            await DeleteContactAsync(contact.Id, ct);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Task DeleteContactsAsync(
            IEnumerable<Contact> contacts,
            CancellationToken ct = default
        )
        {
            return DeleteContactsAsync(contacts.Select(c => c.Id), ct);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
