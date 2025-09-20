using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PsychoChat;
using PsychoChat.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Регистрируем HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:5072") // Ваш API URL
});

// Регистрируем MudBlazor
builder.Services.AddMudServices();

// Регистрируем наши сервисы
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<ChatService>();

// Добавляем аутентификацию
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();