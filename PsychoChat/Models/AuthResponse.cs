namespace PsychoChat.Models
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
