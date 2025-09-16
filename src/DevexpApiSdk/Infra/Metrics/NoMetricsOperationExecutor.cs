using System.Diagnostics;
using DevexpApiSdk.Common.Metrics;

namespace DevexpApiSdk.Metrics
{
    internal class NoMetricsOperationExecutor : IOperationExecutor
    {
        internal NoMetricsOperationExecutor() { }

        public async Task<T> ExecuteAsync<T>(
            string operationName,
            Func<Task<T>> action,
            int? itemCount = null,
            CancellationToken ct = default
        )
        {
            return await action();
        }

        public async Task ExecuteAsync(
            string operationName,
            Func<Task> action,
            int? itemCount = null,
            CancellationToken ct = default
        )
        {
            await action();
        }
    }
}
