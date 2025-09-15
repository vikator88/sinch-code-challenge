using System.Diagnostics;

namespace DevexpApiSdk.Common.Metrics
{
    internal static class OperationExecutor
    {
        internal static async Task<T> ExecuteAsync<T>(
            string operationName,
            Func<Task<T>> action,
            DevexpApiOptions options,
            int? itemCount = null
        )
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = await action();
                sw.Stop();

                options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = operationName,
                        Duration = sw.Elapsed,
                        ItemCount = itemCount,
                        Success = true
                    }
                );

                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = operationName,
                        Duration = sw.Elapsed,
                        ItemCount = itemCount,
                        Success = false,
                        Exception = ex
                    }
                );
                throw;
            }
        }

        internal static async Task ExecuteAsync(
            string operationName,
            Func<Task> action,
            DevexpApiOptions options,
            int? itemCount = null
        )
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await action();
                sw.Stop();

                options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = operationName,
                        Duration = sw.Elapsed,
                        ItemCount = itemCount,
                        Success = true
                    }
                );
            }
            catch (Exception ex)
            {
                sw.Stop();
                options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = operationName,
                        Duration = sw.Elapsed,
                        ItemCount = itemCount,
                        Success = false,
                        Exception = ex
                    }
                );
                throw;
            }
        }
    }
}
