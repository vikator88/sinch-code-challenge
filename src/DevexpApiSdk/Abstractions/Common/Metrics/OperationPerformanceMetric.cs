namespace DevexpApiSdk.Common.Metrics
{
    public class OperationPerformanceMetric
    {
        public string OperationName { get; init; } = string.Empty;
        public TimeSpan Duration { get; init; }
        public int? ItemCount { get; init; }
        public bool Success { get; init; }
        public Exception Exception { get; init; }
    }
}
