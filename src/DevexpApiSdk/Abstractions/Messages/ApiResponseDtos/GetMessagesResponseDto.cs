namespace DevexpApiSdk.Messages.ApiResponseDtos
{
    internal class GetMessagesResponseDto
    {
        public List<MessageDto> Messages { get; set; } = new();

        public MessagesData Data { get; set; } = new();

        public int Page { get; set; }

        public int QuantityPerPage { get; set; }
    }

    internal class MessagesData
    {
        public Dictionary<Guid, ContactDto> Contacts { get; set; } = new();
    }

    internal class MessageDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;

        public Guid To { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }
    }

    internal class ContactDto
    {
        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}
