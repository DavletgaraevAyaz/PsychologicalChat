namespace APIPsychologicalChat.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? TemporaryNickname { get; set; }
    }
}
