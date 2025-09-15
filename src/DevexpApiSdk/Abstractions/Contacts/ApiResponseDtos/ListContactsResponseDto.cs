using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Contacts.ApiResponseDtos
{
    internal class ListContactsResponseDto
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public Contact[] Contacts { get; set; }
    }
}
