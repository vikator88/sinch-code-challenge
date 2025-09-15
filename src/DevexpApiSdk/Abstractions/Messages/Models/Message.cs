using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Messages.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string From { get; set; }
        public Guid To { get; set; }
        public Contact ToContact { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
