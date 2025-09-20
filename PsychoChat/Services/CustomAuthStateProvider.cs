using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace PsychoChat.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;

        public CustomAuthStateProvider(AuthService authService)
        {
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var isAuthenticated = await _authService.IsAuthenticatedAsync();

            if (isAuthenticated)
            {
                var role = await _authService.GetUserRoleAsync();
                var userId = await _authService.GetUserIdAsync();
                var email = await _authService.GetUserEmailAsync();

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role ?? ""),
                new Claim(ClaimTypes.NameIdentifier, userId?.ToString() ?? ""),
                new Claim(ClaimTypes.Email, email ?? ""),
                new Claim(ClaimTypes.Name, email ?? "User")
            };

                var identity = new ClaimsIdentity(claims, "custom");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void NotifyAuthenticationChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
