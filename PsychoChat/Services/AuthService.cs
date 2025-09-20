using PsychoChat.Models;
using System.Net.Http.Json;

namespace PsychoChat.Services
{
    using System.Net.Http.Json;
    using System.Security.Claims;

    public class AuthService
    {
        private const string UserIdKey = "user_id";
        private const string UserRoleKey = "user_role";
        private const string UserEmailKey = "user_email";
        private const string TokenKey = "auth_token";
        private const string IsAnonymousKey = "is_anonymous";

        private readonly LocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        public AuthService(LocalStorageService localStorage, HttpClient httpClient)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
        }

        // Анонимный вход (без регистрации)
        public async Task<bool> AnonymousLoginAsync()
        {
            try
            {
                // Создаем анонимного пользователя через API
                var response = await _httpClient.PostAsync("api/users/create", null);

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();
                    await SaveUserData(user.Id, "Student", "anonymous@user.com", "anonymous-token", true);
                    return true;
                }

                // Если API недоступно, создаем локально
                return await CreateLocalAnonymousUser();
            }
            catch
            {
                // Fallback: создаем анонимного пользователя локально
                return await CreateLocalAnonymousUser();
            }
        }

        private async Task<bool> CreateLocalAnonymousUser()
        {
            await SaveUserData(Guid.NewGuid(), "Student", "anonymous@user.com", "anonymous-token", true);
            return true;
        }

        // Обычная регистрация
        public async Task<AuthResult> RegisterAsync(string email, string role)
        {
            try
            {
                var request = new RegisterRequest
                {
                    Email = email,
                };

                var response = await _httpClient.PostAsJsonAsync("api/Users/create", request);

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                    if (authResponse?.Success == true)
                    {
                        await SaveUserData(authResponse.UserId, authResponse.Role,
                                         authResponse.Email, authResponse.Token, false);
                        return new AuthResult { Success = true };
                    }

                    return new AuthResult { Success = false, Message = authResponse?.Message ?? "Ошибка регистрации" };
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return new AuthResult { Success = false, Message = $"Ошибка сервера: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Ошибка подключения: {ex.Message}" };
            }
        }

        // Обычный вход
        public async Task<AuthResult> LoginAsync(string email, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    Email = email,
                    Password = password
                };

                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                    if (authResponse?.Success == true)
                    {
                        await SaveUserData(authResponse.UserId, authResponse.Role,
                                         authResponse.Email, authResponse.Token, false);
                        return new AuthResult { Success = true };
                    }

                    return new AuthResult { Success = false, Message = authResponse?.Message ?? "Ошибка входа" };
                }
                else
                {
                    return new AuthResult { Success = false, Message = $"Ошибка сервера: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new AuthResult { Success = false, Message = $"Ошибка подключения: {ex.Message}" };
            }
        }

        private async Task SaveUserData(Guid userId, string role, string email, string token, bool isAnonymous)
        {
            await _localStorage.SetItemAsync(UserIdKey, userId.ToString());
            await _localStorage.SetItemAsync(UserRoleKey, role);
            await _localStorage.SetItemAsync(UserEmailKey, email);
            await _localStorage.SetItemAsync(TokenKey, token);
            await _localStorage.SetItemAsync(IsAnonymousKey, isAnonymous);
        }

        public async Task<bool> IsAnonymousAsync()
        {
            return await _localStorage.GetItemAsync<bool>(IsAnonymousKey);
        }

        public async Task<string?> GetUserRoleAsync() => await _localStorage.GetItemAsync<string>(UserRoleKey);
        public async Task<Guid?> GetUserIdAsync() => Guid.TryParse(await _localStorage.GetItemAsync<string>(UserIdKey), out var id) ? id : null;
        public async Task<string?> GetUserEmailAsync() => await _localStorage.GetItemAsync<string>(UserEmailKey);
        public async Task<string?> GetTokenAsync() => await _localStorage.GetItemAsync<string>(TokenKey);
        public async Task<bool> IsAuthenticatedAsync() => !string.IsNullOrEmpty(await GetTokenAsync());

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync(UserIdKey);
            await _localStorage.RemoveItemAsync(UserRoleKey);
            await _localStorage.RemoveItemAsync(UserEmailKey);
            await _localStorage.RemoveItemAsync(TokenKey);
            await _localStorage.RemoveItemAsync(IsAnonymousKey);
        }
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class User
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? TemporaryNickname { get; set; }
    }
}
