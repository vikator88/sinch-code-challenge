namespace DevexpApiSdk.Messages.Models
{
    public class CreateMessageRequest
    {
        public string Content { get; set; }
        public string From { get; set; }
        public CreateMessageContactRequest To { get; set; }
    }

    public class CreateMessageContactRequest
    {
        public Guid Id { get; set; }
    }
}
