namespace DevexpApiSdk.Messages.ApiResponseDtos
{
    internal class GetMessagesResponseDto
    {
        internal List<MessageDto> Messages { get; set; } = new();
        internal MessagesData Data { get; set; } = new();
        internal int Page { get; set; }
        internal int QuantityPerPage { get; set; }
    }

    internal class MessagesData
    {
        internal Dictionary<Guid, ContactDto> Contacts { get; set; } = new();
    }

    internal class MessageDto
    {
        internal Guid Id { get; set; }
        internal string Content { get; set; } = string.Empty;
        internal string From { get; set; } = string.Empty;
        internal Guid To { get; set; }
        internal string Status { get; set; } = string.Empty;
        internal DateTime CreatedAt { get; set; }
        internal DateTime? DeliveredAt { get; set; }
    }

    internal class ContactDto
    {
        internal string Name { get; set; } = string.Empty;
        internal string Phone { get; set; } = string.Empty;
    }
}
