using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Samples
{
    public static class ContactsManagementSample
    {
        public static async Task RunAsync()
        {
            var client = new DevexpApiClient(
                DevexpApiOptionsBuilder
                    .CreateDefault()
                    .WithApiKey("there-is-no-key")
                    .WithOperationProfiler(
                        (metric) =>
                            Console.WriteLine(
                                $"Operation {metric.OperationName} took {metric.Duration.TotalMilliseconds} ms"
                            )
                    )
                    .Build()
            );

            Console.WriteLine("Contacts Management Sample");

            Console.WriteLine("Deleting previous existing contacts if any...");
            // Clean up any previous existing contacts
            var existingContacts = await client.Contacts.GetContactsAsync(1, 100);
            if (existingContacts.Items.Any())
            {
                Console.WriteLine(
                    $"Found {existingContacts.Items.Count} existing contacts. Deleting them..."
                );
                await client.Contacts.DeleteContactsAsync(
                    existingContacts.Items.Select(c => c.Id).ToList()
                );
            }

            Console.WriteLine("Creating 8 dummy contacts...");
            // Create 8 dummy contacts
            var contactsToCreate = Enumerable
                .Range(1, 8)
                .Select(i => new { Name = $"Contact {i}", Phone = $"+3466655500{i}" })
                .ToList();

            var createdContacts = await client.Contacts.AddContactsAsync(
                contactsToCreate
                    .Select(c => new CreateContactRequest { Name = c.Name, Phone = c.Phone })
                    .ToList()
            );

            Console.WriteLine("Listing all contacts...");

            // List all contacts (page size 5)
            var pageNumber = 1;
            IPagedResult<Contact> page;
            do
            {
                page = await client.Contacts.GetContactsAsync(pageNumber, 5);
                foreach (var contact in page.Items)
                {
                    Console.WriteLine($"- {contact.Name} ({contact.Phone})");
                }
                pageNumber++;
            } while (page.Items.Any());

            Console.WriteLine("Deleting all created contacts...");

            // delete all created contacts in bulk
            await client.Contacts.DeleteContactsAsync(createdContacts.Select(c => c.Id).ToList());

            // trying to add a contact with invalid phone number
            try
            {
                Console.WriteLine("Trying to add a contact with invalid phone number...");
                var invalidContact = await client.Contacts.AddContactAsync(
                    "Invalid Contact",
                    "12345"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Expected error adding contact: {ex.Message}");
            }

            // trying to add twice the same contact
            Console.WriteLine("Trying to add twice the same contact...");
            Contact contact1 = new Contact();
            try
            {
                contact1 = await client.Contacts.AddContactAsync(
                    "Duplicate Contact",
                    "+34666555000"
                );
                var contact2 = await client.Contacts.AddContactAsync(
                    "Duplicate Contact",
                    "+34666555000"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Expected error adding contact: {ex.Message}");

                await client.Contacts.DeleteContactAsync(contact1);
            }

            // Add one contact and update it
            Console.WriteLine("Adding one contact and updating it...");
            var newContact = await client.Contacts.AddContactAsync(
                "Updatable Contact",
                "+34666555001"
            );
            Console.WriteLine($"Contact added: {newContact.Name} ({newContact.Phone})");

            newContact = await client.Contacts.UpdateContactAsync(
                newContact.Id,
                "Updated Contact",
                "+34666555002"
            );
            Console.WriteLine($"Contact updated: {newContact.Name} ({newContact.Phone})");

            // Clean up
            await client.Contacts.DeleteContactAsync(newContact);
        }
    }
}
