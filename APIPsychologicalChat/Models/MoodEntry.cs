namespace APIPsychologicalChat.Models
{
    public class MoodEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int MoodLevel { get; set; } // Шкала от 1 до 10
        public string? Notes { get; set; } // Опциональные заметки
        public DateTime CreatedAt { get; set; }
    }
}
