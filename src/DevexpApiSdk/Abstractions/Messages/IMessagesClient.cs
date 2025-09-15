using DevexpApiSdk.Common;
using DevexpApiSdk.Contacts.Models;
using DevexpApiSdk.Messages.Models;

namespace DevexpApiSdk.Messages
{
    /// <summary>
    /// Provides a strongly-typed client for interacting with the Messages resource domain
    /// of the Devexp API.
    /// </summary>
    /// <remarks>
    /// This client encapsulates HTTP operations for sending messages, retrieving messages by ID,
    /// and listing messages. It leverages <see cref="IDevexpApiHttpClient"/> for transport
    /// and DTO mappers for response transformation.
    /// </remarks>
    public interface IMessagesClient
    {
        /// <summary>
        /// Retrieves a paged collection of messages.
        /// </summary>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>
        /// A paged result containing <see cref="Message"/> entities
        /// corresponding to messages retrieved from the API.
        /// </returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<IPagedResult<Message>> GetMessagesAsync(CancellationToken ct = default);

        /// <summary>
        /// Retrieves a single message by its unique identifier.
        /// </summary>
        /// <param name="messageId">The unique identifier of the message.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The requested <see cref="Message"/>.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Message> GetMessageByIdAsync(Guid messageId, CancellationToken ct = default);

        /// <summary>
        /// Sends a message to a specific contact identified by its <see cref="Guid"/>.
        /// </summary>
        /// <param name="from">The sender identifier (e.g., phone number or alphanumeric ID).</param>
        /// <param name="messageContent">The message body content.</param>
        /// <param name="toContactId">The unique identifier of the recipient contact.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The created <see cref="Message"/> including delivery metadata.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Message> SendMessageAsync(
            string from,
            string messageContent,
            Guid toContactId,
            CancellationToken ct = default
        );

        /// <summary>
        /// Sends a message to a specific contact.
        /// </summary>
        /// <param name="from">The sender identifier (e.g., phone number or alphanumeric ID).</param>
        /// <param name="messageContent">The message body content.</param>
        /// <param name="toContact">The recipient <see cref="Contact"/> entity.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The created <see cref="Message"/> including delivery metadata.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Message> SendMessageAsync(
            string from,
            string messageContent,
            Contact toContact,
            CancellationToken ct = default
        );

        /// <summary>
        /// Sends a message using a request model.
        /// </summary>
        /// <param name="createMessageRequest">The request object containing sender, content, and recipient.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>The created <see cref="Message"/> including delivery metadata.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<Message> SendMessageAsync(
            CreateMessageRequest createMessageRequest,
            CancellationToken ct = default
        );

        /// <summary>
        /// Sends the same message to multiple contacts.
        /// </summary>
        /// <param name="from">The sender identifier (e.g., phone number or alphanumeric ID).</param>
        /// <param name="messageContent">The message body content.</param>
        /// <param name="toContacts">An array of recipient <see cref="Contact"/> entities.</param>
        /// <param name="ct">An optional cancellation token.</param>
        /// <returns>A read-only list of <see cref="Message"/> objects, one for each recipient.</returns>
        /// <exception cref="ApiAuthException">Thrown if authentication fails.</exception>
        /// <exception cref="ApiNotFoundException">Thrown if the contact is not found.</exception>
        /// <exception cref="ApiServerException">Thrown if the server encounters an error (5XX error).</exception>
        /// <exception cref="ApiException">Thrown if the API returns an unexpected error.</exception>
        Task<IReadOnlyList<Message>> SendMessageAsync(
            string from,
            string messageContent,
            Contact[] toContacts,
            CancellationToken ct = default
        );
    }
}
