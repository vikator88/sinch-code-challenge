using Newtonsoft.Json;

namespace DevexpApiSdk.Infra.Utils
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

                // normalizar: si `error` está presente, úsalo como Message
                if (!string.IsNullOrEmpty(dto.Error))
                {
                    dto.Message = dto.Error;
                }

                // si no hay ni message, ni error, ni id → devolvemos null
                if (string.IsNullOrEmpty(dto.Message) && string.IsNullOrEmpty(dto.Id))
                    return null;

                return dto;
            }
            catch (JsonException)
            {
                return null; // JSON inválido
            }
        }
    }
}
