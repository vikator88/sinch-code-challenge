namespace DevexpApiSdk.Contacts.Models
{
    /// <summary>
    /// Represents a contact entity in the Devexp API.
    /// </summary>
    /// <remarks>
    /// A contact is uniquely identified by <see cref="Id"/> and contains
    /// basic attributes such as <see cref="Name"/> and <see cref="Phone"/>.
    /// </remarks>
    public record Contact
    {
        /// <summary>
        /// Gets or sets the unique identifier of the contact.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the contact.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the contact in format E.164.
        /// </summary>
        /// /// <remarks>
        /// If the phone number is not in valid E.164 format,
        /// an <see cref="DevexpApiSdk.Common.Exceptions.InvalidPhoneNumberException"/>
        /// will be thrown during validation.
        /// </remarks>
        public string Phone { get; set; }
    }
}
