namespace DevexpApiSdk.Contacts.Models
{
    /// <summary>
    /// Represents the payload used to create a new contact in the Devexp API.
    /// </summary>
    /// <remarks>
    /// This request model is consumed by the <c>Contacts</c> endpoint when
    /// invoking operations such as <c>AddContactAsync</c>.
    /// </remarks>
    public class CreateContactRequest
    {
        /// <summary>
        /// Gets or sets the display name of the contact to create.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the contact's phone number in E.164 format.
        /// </summary>
        /// <remarks>
        /// If the phone number is not in valid E.164 format,
        /// an <see cref="DevexpApiSdk.Common.Exceptions.InvalidPhoneNumberException"/>
        /// will be thrown during validation.
        /// </remarks>
        public string Phone { get; set; }
    }
}
