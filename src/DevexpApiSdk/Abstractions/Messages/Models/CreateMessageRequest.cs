namespace DevexpApiSdk.Messages.Models
{
    public record CreateMessageRequest
    {
        /// <summary>
        /// Gets or sets the body content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <remarks>
        /// This may be an alphanumeric sender ID or a phone number
        /// (depending on API configuration).
        /// </remarks>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the recipient information for the message.
        /// </summary>
        /// <remarks>
        /// The <see cref="To"/> property must contain a valid
        /// <see cref="CreateMessageContactRequest"/> referencing an existing contact.
        /// </remarks>
        public CreateMessageContactRequest To { get; set; }
    }

    /// <summary>
    /// Represents the recipient of a message by contact identifier.
    /// </summary>
    /// <remarks>
    /// This model is nested within <see cref="CreateMessageRequest"/>
    /// to reference an existing contact as the recipient.
    /// </remarks>
    public record CreateMessageContactRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the recipient contact.
        /// </summary>
        public Guid Id { get; set; }
    }
}
