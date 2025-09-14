using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Contacts.ApiResponseDtos
{
    internal class ListContactsResponseDto
    {
        internal int PageSize { get; set; }
        internal int PageNumber { get; set; }
        internal Contact[] ContactList { get; set; }
    }
}
