using System.Collections.Concurrent;
using DevexpApiSdk.Common;
using DevexpApiSdk.Common.Metrics;
using DevexpApiSdk.Contacts.ApiResponseDtos;
using DevexpApiSdk.Contacts.Mappers;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Http;

namespace DevexpApiSdk.Contacts
{
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

        public async Task<Contact> AddContactAsync(
            CreateContactRequest createContactRequest,
            CancellationToken ct = default
        )
        {
            return await AddContactAsync(createContactRequest.Name, createContactRequest.Phone, ct);
        }

        public async Task<Contact> AddContactAsync(
            string name,
            string phone,
            CancellationToken ct = default
        )
        {
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
                    if (!_options.EnableBulkOperations)
                    {
                        var results = new List<Contact>();
                        foreach (var c in contacts)
                        {
                            results.Add(await AddContactAsync(c.Name, c.Phone, ct));
                        }
                        return results;
                    }

                    var resultsBag = new ConcurrentBag<Contact>();

                    await Parallel.ForEachAsync(
                        contacts,
                        new ParallelOptions
                        {
                            MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism,
                            CancellationToken = ct
                        },
                        async (c, token) =>
                        {
                            var contact = await AddContactAsync(c.Name, c.Phone, token);
                            resultsBag.Add(contact);
                        }
                    );

                    return resultsBag.ToList();
                },
                _options
            );
        }

        public async Task DeleteContactAsync(Guid contactId, CancellationToken ct = default)
        {
            await _http.SendAsync(HttpMethod.Delete, $"{_resourcePath}/{contactId}", null, ct);
        }

        public async Task DeleteContactAsync(Contact contact, CancellationToken ct = default)
        {
            await DeleteContactAsync(contact.Id, ct);
        }

        public async Task DeleteContactsAsync(
            IEnumerable<Guid> contactIds,
            CancellationToken ct = default
        )
        {
            if (!_options.EnableBulkOperations)
            {
                foreach (var c in contactIds)
                {
                    await DeleteContactAsync(c, ct);
                }
            }

            await Parallel.ForEachAsync(
                contactIds,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism,
                    CancellationToken = ct
                },
                async (c, token) =>
                {
                    await DeleteContactAsync(c, token);
                }
            );
        }

        public Task DeleteContactsAsync(
            IEnumerable<Contact> contacts,
            CancellationToken ct = default
        )
        {
            return DeleteContactsAsync(contacts.Select(c => c.Id), ct);
        }

        public async Task<IPagedResult<Contact>> GetContactsAsync(
            int pageNumber = 1,
            CancellationToken ct = default
        )
        {
            return await GetContactsAsync(pageNumber, _options.DefaultPageSize, ct);
        }

        public async Task<IPagedResult<Contact>> GetContactsAsync(
            int pageNumber,
            int pageSize,
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

        public async Task<Contact> UpdateContactAsync(
            Guid contactId,
            string contactName,
            string contactPhone,
            CancellationToken ct = default
        )
        {
            var body = new { contactName, contactPhone };
            var response = await _http.SendAsync<Contact>(
                HttpMethod.Patch,
                $"{_resourcePath}/{contactId}",
                body,
                ct
            );

            return response.Data;
        }

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

        public async Task<IReadOnlyList<Contact>> UpdateContactsAsync(
            IEnumerable<Contact> updateContactRequests,
            CancellationToken ct = default
        )
        {
            if (!_options.EnableBulkOperations)
            {
                var results = new List<Contact>();
                foreach (var c in updateContactRequests)
                {
                    results.Add(await UpdateContactAsync(c, ct));
                }
                return results;
            }

            var resultsBag = new ConcurrentBag<Contact>();

            await Parallel.ForEachAsync(
                updateContactRequests,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism,
                    CancellationToken = ct
                },
                async (c, token) =>
                {
                    var contact = await UpdateContactAsync(c, token);
                    resultsBag.Add(contact);
                }
            );

            return resultsBag.ToList();
        }
    }
}
