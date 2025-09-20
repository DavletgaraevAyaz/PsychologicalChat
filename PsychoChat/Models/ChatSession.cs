namespace PsychoChat.Models
{
    public class ChatSession
    {
        public Guid Id { get; set; }
        public string PsychologistName { get; set; } = string.Empty;
        public string PsychologistSpecialization { get; set; } = string.Empty;
        public DateTime LastMessageTime { get; set; }
        public string LastMessagePreview { get; set; } = string.Empty;
        public int UnreadCount { get; set; }
        public bool IsOnline { get; set; }
    }
}
