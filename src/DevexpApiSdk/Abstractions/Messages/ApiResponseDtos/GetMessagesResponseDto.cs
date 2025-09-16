using DevexpApiSdk.Messages.Models;

namespace DevexpApiSdk.Messages.ApiResponseDtos
{
    // This set of records are internal because they're only used for deserialization within the SDK and not exposed to consumers.
    // Properties are public to allow set them during deserialization.
    internal record GetMessagesResponseDto
    {
        public List<MessageDto> Messages { get; set; } = new();

        public MessagesData Data { get; set; } = new();

        public int Page { get; set; }

        public int QuantityPerPage { get; set; }
    }

    internal record MessagesData
    {
        public Dictionary<Guid, ContactDto> Contacts { get; set; } = new();
    }

    internal record MessageDto
    {
        public Guid Id { get; set; }

        public string Content { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;

        public Guid To { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DeliveredAt { get; set; }
    }

    internal record ContactDto
    {
        public string Name { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;
    }
}
