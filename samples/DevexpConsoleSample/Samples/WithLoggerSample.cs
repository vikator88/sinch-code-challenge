using DevexpApiSdk.Common;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Samples
{
    public static class WithLoggerSample
    {
        public static async Task RunAsync()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Information)
                    .AddSimpleConsole(options =>
                    {
                        options.SingleLine = true;
                        options.TimestampFormat = "HH:mm:ss ";
                    });
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();

            var options = DevexpApiOptionsBuilder
                .CreateDefault()
                .WithApiKey("there-is-no-key")
                .EnableLogging(logger)
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
