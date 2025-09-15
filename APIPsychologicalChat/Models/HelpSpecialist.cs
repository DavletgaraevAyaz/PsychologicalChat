namespace APIPsychologicalChat.Models
{
    public class HelpSpecialist
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? WorkHours { get; set; }
    }
}
