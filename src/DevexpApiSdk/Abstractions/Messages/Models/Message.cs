using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Messages.Models
{
    /// <summary>
    /// Represents a message entity in the Devexp API.
    /// </summary>
    /// <remarks>
    /// A message contains content, sender and recipient information,
    /// delivery status, and timestamps for creation and delivery.
    /// </remarks>
    public class Message
    {
        /// <summary>
        /// Gets or sets the unique identifier of the message.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the body content of the message.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the recipient contact.
        /// </summary>
        public Guid To { get; set; }

        /// <summary>
        /// Gets or sets the recipient contact entity, if available.
        /// </summary>
        public Contact ToContact { get; set; }

        /// <summary>
        /// Gets or sets the current delivery status of the message.
        /// </summary>
        /// <remarks>
        /// Typical values include <c>"queued"</c>, <c>"success"</c>, or <c>"error"</c>.
        /// </remarks>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the message was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the message was successfully delivered, if applicable.
        /// </summary>
        /// <remarks>
        /// This property is <c>null</c> if the message has not yet been delivered.
        /// </remarks>
        public DateTime? DeliveredAt { get; set; }
    }
}
