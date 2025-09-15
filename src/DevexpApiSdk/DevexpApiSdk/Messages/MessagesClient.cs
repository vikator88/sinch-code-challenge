using System.Collections.Concurrent;
using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Http;
using DevexpApiSdk.Messages.ApiResponseDtos;
using DevexpApiSdk.Messages.Mappers;
using DevexpApiSdk.Messages.Models;

namespace DevexpApiSdk.Messages
{
    /// <inheritdoc/>
    internal class MessagesClient : IMessagesClient
    {
        private readonly IDevexpApiHttpClient _http;
        private readonly string _resourcePath = "/messages";
        private readonly DevexpApiOptions _options;

        internal MessagesClient(IDevexpApiHttpClient http, DevexpApiOptions options)
        {
            _http = http;
            _options = options;
        }

        /// <inheritdoc/>
        public async Task<IPagedResult<Message>> GetMessagesAsync(CancellationToken ct = default)
        {
            var response = await _http.SendAsync<GetMessagesResponseDto>(
                HttpMethod.Get,
                $"{_resourcePath}",
                null,
                ct
            );

            return MessagesListResponseMapper.MapToPagedResult(response.Data!);
        }

        /// <inheritdoc/>
        public async Task<Message> GetMessageByIdAsync(
            Guid messageId,
            CancellationToken ct = default
        )
        {
            var response = await _http.SendAsync<Message>(
                HttpMethod.Get,
                $"{_resourcePath}/{messageId}",
                null,
                ct
            );
            return response.Data!;
        }

        /// <inheritdoc/>
        public async Task<Message> SendMessageAsync(
            string from,
            string messageContent,
            Guid toContactId,
            CancellationToken ct = default
        )
        {
            var createMessageRequest = new CreateMessageRequest
            {
                From = from,
                Content = messageContent,
                To = new CreateMessageContactRequest { Id = toContactId }
            };

            return await SendMessageAsync(createMessageRequest, ct);
        }

        /// <inheritdoc/>
        public async Task<Message> SendMessageAsync(
            string from,
            string messageContent,
            Contact toContact,
            CancellationToken ct = default
        )
        {
            var createMessageRequest = new CreateMessageRequest
            {
                From = from,
                Content = messageContent,
                To = new CreateMessageContactRequest { Id = toContact.Id }
            };

            return await SendMessageAsync(createMessageRequest, ct);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<Message>> SendMessageAsync(
            string from,
            string messageContent,
            Contact[] toContacts,
            CancellationToken ct = default
        )
        {
            var results = new List<Message>();
            foreach (var c in toContacts)
            {
                results.Add(await SendMessageAsync(from, messageContent, c, ct));
            }
            return results;
        }

        /// <inheritdoc/>
        public async Task<Message> SendMessageAsync(
            CreateMessageRequest createMessageRequest,
            CancellationToken ct = default
        )
        {
            var response = await _http.SendAsync<Message>(
                HttpMethod.Post,
                $"{_resourcePath}",
                createMessageRequest,
                ct
            );

            return response.Data!;
        }
    }
}
