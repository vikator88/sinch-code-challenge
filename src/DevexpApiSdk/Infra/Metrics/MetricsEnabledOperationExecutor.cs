using System.Diagnostics;
using DevexpApiSdk.Common.Metrics;

namespace DevexpApiSdk.Metrics
{
    internal class MetricsEnabledOperationExecutor : IOperationExecutor
    {
        private readonly Action<OperationPerformanceMetric> _onCompleted;

        internal MetricsEnabledOperationExecutor(Action<OperationPerformanceMetric> onCompleted)
        {
            _onCompleted = onCompleted;
        }

        public async Task<T> ExecuteAsync<T>(
            string operationName,
            Func<Task<T>> action,
            int? itemCount = null,
            CancellationToken ct = default
        )
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = await action();
                sw.Stop();

                _onCompleted?.Invoke(
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
                _onCompleted?.Invoke(
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

        public async Task ExecuteAsync(
            string operationName,
            Func<Task> action,
            int? itemCount = null,
            CancellationToken ct = default
        )
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await action();
                sw.Stop();

                _onCompleted?.Invoke(
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
                _onCompleted?.Invoke(
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
