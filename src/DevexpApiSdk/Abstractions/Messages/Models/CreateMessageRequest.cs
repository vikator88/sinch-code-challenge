using DevexpApiSdk.Contacts.Models;

namespace DevexpApiSdk.Messages.Models
{
    public class CreateMessageRequest
    {
        public string Content { get; set; }
        public string From { get; set; }
        public Contact To { get; set; }
    }
}
