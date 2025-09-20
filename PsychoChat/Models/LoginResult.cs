namespace PsychoChat.Models
{
    public class LoginResult
    {
        public Guid UserId { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
