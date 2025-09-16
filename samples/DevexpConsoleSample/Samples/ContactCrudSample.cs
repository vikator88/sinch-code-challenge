using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Samples
{
    public static class ContactCrudSample
    {
        public static async Task RunAsync(CancellationToken cancellationToken)
        {
            IDevexpApiClient client = new DevexpApiClient("there-is-no-key");

            // Create a new contact request
            var newContactRequest = new CreateContactRequest()
            {
                Name = "Fernando Torres",
                Phone = "+34666555444",
            };

            Contact createdContact = await client.Contacts.AddContactAsync(
                newContactRequest,
                cancellationToken
            );

            Console.WriteLine($"Created Contact: {createdContact.Name}, ID: {createdContact.Id}");

            // Retrieve the created contact by its ID
            Contact retrievedContact = await client.Contacts.GetContactByIdAsync(
                createdContact.Id,
                cancellationToken
            );
            Console.WriteLine(
                $"Retrieved Contact: {retrievedContact.Name}, Phone: {retrievedContact.Phone}"
            );

            // Update the contact's information
            retrievedContact.Phone = "+34666000111";
            Contact updatedContact = await client.Contacts.UpdateContactAsync(
                retrievedContact,
                cancellationToken
            );
            Console.WriteLine(
                $"Updated Contact: {updatedContact.Name}, New Phone: {updatedContact.Phone}"
            );

            // Delete the contact
            await client.Contacts.DeleteContactAsync(updatedContact.Id, cancellationToken);
            Console.WriteLine($"Deleted Contact with ID: {updatedContact.Id}");
        }
    }
}
