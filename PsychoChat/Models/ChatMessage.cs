namespace PsychoChat.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid ChatId { get; set; }
        public string Sender { get; set; } = string.Empty; // "user" или "psychologist"
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
    }
}
