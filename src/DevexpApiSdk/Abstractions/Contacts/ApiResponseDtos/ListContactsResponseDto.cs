using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Contacts.ApiResponseDtos
{
    // This class is internal because it's only used for deserialization within the SDK and not exposed to consumers.
    // Properties are public to allow set them during deserialization.
    internal record ListContactsResponseDto
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public Contact[] Contacts { get; set; }
    }
}
