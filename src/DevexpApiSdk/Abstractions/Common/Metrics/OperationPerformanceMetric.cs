namespace DevexpApiSdk.Common.Metrics
{
    /// <summary>
    /// Represents performance metrics collected during the execution of an API operation.
    /// </summary>
    /// <remarks>
    /// This model is typically passed to the <see cref="DevexpApiOptions.OnOperationCompleted"/> callback when metrics are enabled.
    /// </remarks>
    public record OperationPerformanceMetric
    {
        /// <summary>
        /// Gets the logical name of the operation being executed.
        /// </summary>
        /// <remarks>
        /// For example, <c>"Contacts.AddContact"</c> or <c>"Messages.SendMessage"</c>.
        /// </remarks>
        public string OperationName { get; init; } = string.Empty;

        /// <summary>
        /// Gets the total execution duration of the operation.
        /// </summary>
        public TimeSpan Duration { get; init; }

        /// <summary>
        /// Gets the number of items processed or returned by the operation, if applicable.
        /// </summary>
        /// <remarks>
        /// For non-collection operations, this property may be <c>null</c>.
        /// </remarks>
        public int? ItemCount { get; init; }

        /// <summary>
        /// Gets a value indicating whether the operation completed successfully.
        /// </summary>
        public bool Success { get; init; }

        /// <summary>
        /// Gets the exception thrown during the operation, if any.
        /// </summary>
        /// <remarks>
        /// If <see cref="Success"/> is <c>true</c>, this property will be <c>null</c>.
        /// </remarks>
        public Exception Exception { get; init; }
    }
}
