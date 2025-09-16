using DevexpApiSdk.Common;
using Microsoft.Extensions.Logging;

namespace DevexpApiSdk.Samples
{
    public static class WithLoggerSample
    {
        public static async Task RunAsync(CancellationToken ct = default)
        {
            // Setting up a console logger
            // Any Logger implementation can be used here (Serilog, NLog, etc.) as long as it implements Microsoft.Extensions.Logging.ILogger
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
