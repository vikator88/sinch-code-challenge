using System.Diagnostics;
using System.Reflection;

namespace DevexpApiSdk.Common.Metrics
{
    internal class ProfilingProxy<T> : DispatchProxy
        where T : class
    {
        internal T Inner { get; set; } = default!;
        internal DevexpApiOptions Options { get; set; } = default!;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = targetMethod!.Invoke(Inner, args!);

                if (result is Task task)
                {
                    return InterceptTask(task, targetMethod.Name, sw);
                }

                sw.Stop();
                Options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = $"{typeof(T).Name}.{targetMethod.Name}",
                        Duration = sw.Elapsed,
                        Success = true
                    }
                );

                return result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                Options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = $"{typeof(T).Name}.{targetMethod!.Name}",
                        Duration = sw.Elapsed,
                        Success = false,
                        Exception = ex
                    }
                );

                throw;
            }
        }

        private async Task InterceptTask(Task task, string opName, Stopwatch sw)
        {
            try
            {
                await task;
                sw.Stop();
                Options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = $"{typeof(T).Name}.{opName}",
                        Duration = sw.Elapsed,
                        Success = true
                    }
                );
            }
            catch (Exception ex)
            {
                sw.Stop();
                Options.OnOperationCompleted?.Invoke(
                    new OperationPerformanceMetric
                    {
                        OperationName = $"{typeof(T).Name}.{opName}",
                        Duration = sw.Elapsed,
                        Success = false,
                        Exception = ex
                    }
                );

                throw;
            }
        }
    }
}
