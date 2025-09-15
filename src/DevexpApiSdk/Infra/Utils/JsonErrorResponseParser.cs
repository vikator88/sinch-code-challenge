namespace DevexpApiSdk.Infra.Utils
{
    using System.Text.Json;
    using DevexpApiSdk.Abstractions.Common.ApiResponseDtos;

    internal static class JsonErrorResponseParser
    {
        internal static ErrorResponseDto Parse(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(json);

                string message = null;
                string id = null;

                if (doc.RootElement.TryGetProperty("message", out var msgProp))
                {
                    message = msgProp.GetString();
                }

                if (doc.RootElement.TryGetProperty("id", out var idProp))
                {
                    id = idProp.GetString();
                }

                // si ninguno de los dos campos aparece → devolvemos null
                if (message == null && id == null)
                    return null;

                // devolvemos el objeto con los campos encontrados
                return new ErrorResponseDto { Message = message, Id = id };
            }
            catch (JsonException)
            {
                // body no es JSON válido
                return null;
            }
        }
    }
}
