using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Messages.ApiResponseDtos;
using DevexpApiSdk.Messages.Models;

namespace DevexpApiSdk.Messages.Mappers
{
    internal static class MessagesListResponseMapper
    {
        private static readonly Dictionary<string, MessageStatus> _statusMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["queued"] = MessageStatus.QUEUED,
                ["delivered"] = MessageStatus.DELIVERED,
                ["failed"] = MessageStatus.FAILED
            };

        internal static IPagedResult<Message> MapToPagedResult(this GetMessagesResponseDto dto)
        {
            // Map messages and rich them with contact info
            var messages = dto
                .Messages.Select(m => new Message
                {
                    Id = m.Id,
                    From = m.From,
                    To = m.To,
                    ToContact = dto.Data.Contacts.TryGetValue(m.To, out var contactDto)
                        ? new Contact { Name = contactDto.Name, Phone = contactDto.Phone }
                        : null,
                    Content = m.Content,
                    Status = _statusMap.TryGetValue(m.Status, out var status) ? status : null,
                    CreatedAt = m.CreatedAt,
                    DeliveredAt = m.DeliveredAt
                })
                .ToArray();

            return new PagedResult<Message>(messages, dto.Page, dto.QuantityPerPage);
        }
    }
}
