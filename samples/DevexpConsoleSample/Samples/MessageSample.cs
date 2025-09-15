using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Samples
{
    public static class MessageSample
    {
        public static async Task RunAsync(CancellationToken cancellationToken)
        {
            IDevexpApiClient client = new DevexpApiClient("there-is-no-key");

            // Adding a contact to send a message to
            var contact = await client.Contacts.AddContactAsync(
                "Victor Tester",
                "+34666555777",
                cancellationToken
            );

            Console.WriteLine($"Created Contact: {contact.Name}, ID: {contact.Id}");

            // Sending a message to the contact
            var message = await client.Messages.SendMessageAsync(
                "ConsoleApplication",
                "Hello from DevexpApiSdk!",
                contact.Id,
                cancellationToken
            );

            Console.WriteLine($"Sent Message: {message.Id}. Status: {message.Status}");

            // Clean up: Deleting the contact
            await client.Contacts.DeleteContactAsync(contact.Id, cancellationToken);
            Console.WriteLine($"Deleted Contact with ID: {contact.Id}");
        }
    }
}
