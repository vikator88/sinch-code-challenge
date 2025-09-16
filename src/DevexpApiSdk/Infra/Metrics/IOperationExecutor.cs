namespace DevexpApiSdk.Metrics
{
    internal interface IOperationExecutor
    {
        Task<T> ExecuteAsync<T>(
            string operationName,
            Func<Task<T>> action,
            int? itemCount = null,
            CancellationToken ct = default
        );

        Task ExecuteAsync(
            string operationName,
            Func<Task> action,
            int? itemCount = null,
            CancellationToken ct = default
        );
    }
}
