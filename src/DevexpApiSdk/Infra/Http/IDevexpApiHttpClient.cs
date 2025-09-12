using DevexpApiSdk.Common;

namespace DevexpApiSdk.Http
{
    internal interface IDevexpApiHttpClient : IDisposable
    {
        Task<DevexpApiResponse<T>> SendAsync<T>(
            HttpMethod method,
            string path,
            object body = null,
            CancellationToken ct = default
        );

        Task<DevexpApiResponse> SendAsync(
            HttpMethod method,
            string path,
            object body = null,
            CancellationToken ct = default
        );
    }
}
