using DevexpApiSdk.Common;

namespace DevexpApiSdk.Samples
{
    public static class WithLoggerProfilerSample
    {
        public static async Task RunAsync(CancellationToken ct = default)
        {
            /// Settingup a callback to log operation durations
            /// This is in addition to logging request/response info via ILogger
            /// This profiler callback will be invoked after each operation completes
            /// with the operation name and duration
            /// This makes easy to attach any custom profiler or metrics collector like Application Insights, Prometheus, etc.
            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("there-is-no-key")
                .WithOperationProfiler(
                    (metric) =>
                    {
                        /// Just check the output to analyze metrics
                        Console.WriteLine(
                            $"Operation '{metric.OperationName}' took {metric.Duration.TotalMilliseconds} ms"
                        );
                    }
                )
                .Build();

            var client = new DevexpApiClient(options);

            // Adding a contact to send a message to
            var contact = await client.Contacts.AddContactAsync(
                "Cholo Simeone",
                "+34666555000",
                ct
            );

            // Sending a message to the contact
            var message = await client.Messages.SendMessageAsync(
                "ConsoleApplication",
                "Hello from DevexpApiSdk!",
                contact.Id,
                ct
            );

            // Clean up: Deleting the contact
            await client.Contacts.DeleteContactAsync(contact.Id, ct);
        }
    }
}
