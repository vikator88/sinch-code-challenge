using DevexpApiSdk.Common;

namespace DevexpApiSdk.Samples
{
    public static class WithLoggerProfilerSample
    {
        public static async Task RunAsync()
        {
            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("there-is-no-key")
                .WithOperationProfiler(
                    (metric) =>
                    {
                        Console.WriteLine(
                            $"Operation '{metric.OperationName}' took {metric.Duration.TotalMilliseconds} ms"
                        );
                    }
                )
                .Build();

            var client = new DevexpApiClient(options);

            // Adding a contact to send a message to
            var contact = await client.Contacts.AddContactAsync("Test Contact", "+34666555000");

            // Sending a message to the contact
            var message = await client.Messages.SendMessageAsync(
                "ConsoleApplication",
                "Hello from DevexpApiSdk!",
                contact.Id
            );

            // Clean up: Deleting the contact
            await client.Contacts.DeleteContactAsync(contact.Id);
        }
    }
}
