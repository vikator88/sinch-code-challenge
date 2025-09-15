namespace DevexpApiSdk.Common.ApiResponseDtos
{
    // This class is internal because it's only used for deserialization within the SDK and not exposed to consumers.
    // Properties are public to allow set them during deserialization.
    internal class ErrorResponseDto
    {
        public string Message { get; set; }
        public string Id { get; set; }
        public string Error { get; set; }
    }
}
