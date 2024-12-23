using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskBox.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<HttpClient>();

builder.Services.AddAuthorizationCore();
builder.Services.AddApiAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
