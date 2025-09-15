using DevexpApiSdk.Common;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Samples
{
    public static class MessageSenderSample
    {
        public static async Task RunAsync()
        {
            Console.WriteLine("Running Message Sender Sample...");

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

            var client = new DevexpApiClient(
                DevexpApiOptionsBuilder
                    .CreateDefault()
                    .WithApiKey("there-is-no-key")
                    .EnableLogging(logger)
                    .Build()
            );

            Console.WriteLine("Contacts Management Sample");

            // Adding a contact to send a message to
            var contact = await client.Contacts.AddContactAsync("Message Receiver", "+34666555000");
            Console.WriteLine($"Contact created: {contact.Name} ({contact.Phone})");

            // Sending a message to the contact
            var message = await client.Messages.SendMessageAsync(
                "ConsoleApplication",
                "Hello from DevexpApiSdk!",
                contact.Id
            );
            Console.WriteLine($"Message sent: {message.Content}");
        }
    }
}
