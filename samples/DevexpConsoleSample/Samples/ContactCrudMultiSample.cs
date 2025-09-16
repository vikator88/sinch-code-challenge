using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Samples
{
    public static class ContactCrudMultiSample
    {
        public static async Task RunAsync(CancellationToken cancellationToken)
        {
            IDevexpApiClient client = new DevexpApiClient("there-is-no-key");

            // Create 4 newContact requests
            var newContactRequests = new List<CreateContactRequest>
            {
                new CreateContactRequest() { Name = "Julian Alvarez", Phone = "+34666555441" },
                new CreateContactRequest() { Name = "Antoine Griezmann", Phone = "+34666555442" },
                new CreateContactRequest() { Name = "Rodrigo De Paul", Phone = "+34666555443" },
                new CreateContactRequest() { Name = "Jan Oblak", Phone = "+34666555444" },
            };

            // Add multiple contacts
            IReadOnlyList<Contact> createdContacts = await client.Contacts.AddContactsAsync(
                newContactRequests,
                cancellationToken
            );

            // Create a contact without model

            var name = "Cholo Simeone";
            var phone = "+34666555445";
            Contact createdContact = await client.Contacts.AddContactAsync(
                name,
                phone,
                cancellationToken
            );
            // list all created contacts
            Console.WriteLine("Created Contacts:");

            IPagedResult<Contact> allContacts = await client.Contacts.GetContactsAsync();
            foreach (var contact in allContacts.Items)
            {
                Console.WriteLine($"- {contact.Name}, ID: {contact.Id}, Phone: {contact.Phone}");
            }

            // Delete all created contacts
            var allContactIds = createdContacts.Select(c => c.Id).ToList();
            await client.Contacts.DeleteContactsAsync(allContactIds, cancellationToken);

            Console.WriteLine("Deleted all created contacts.");
        }
    }
}
