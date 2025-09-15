using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.ApiResponseDtos;
using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Contacts.Mappers
{
    internal static class ListContactsResponseMapper
    {
        internal static IPagedResult<Contact> MapToPagedResult(this ListContactsResponseDto dto)
        {
            return new PagedResult<Contact>(dto.Contacts, dto.PageNumber, dto.PageSize);
        }
    }
}
