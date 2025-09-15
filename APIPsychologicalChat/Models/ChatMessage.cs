namespace APIPsychologicalChat.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // Анонимный ID пользователя
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public bool IsFromSupport { get; set; } // От пользователя или от психолога
    }
}
