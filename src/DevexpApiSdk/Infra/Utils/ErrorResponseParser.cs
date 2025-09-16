using DevexpApiSdk.Common.ApiResponseDtos;
using Newtonsoft.Json;

namespace DevexpApiSdk.Utils
{
    internal static class ErrorResponseParser
    {
        internal static ErrorResponseDto Parse(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                var dto = JsonConvert.DeserializeObject<ErrorResponseDto>(json);

                if (dto == null)
                    return null;

                // Normalize: if error has value, use it as message (ApiExceptionFactory uses Message property)
                if (!string.IsNullOrEmpty(dto.Error))
                {
                    dto.Message = dto.Error;
                }

                // If there's neither a message, nor an error, nor an id return null
                if (string.IsNullOrEmpty(dto.Message) && string.IsNullOrEmpty(dto.Id))
                    return null;

                return dto;
            }
            catch (JsonException)
            {
                return null; // Invalid JSON
            }
        }
    }
}
