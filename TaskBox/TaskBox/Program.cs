using TaskBox.Client.Pages;
using TaskBox.Components;
using DatabaseConnection.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using TaskBox.Interfaces;
using TaskBox.Repositories;
using Microsoft.AspNetCore.Components.Authorization;
using TaskBox.Components.Account;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

//Adds Database Connection
builder.Services.AddScoped<IDatabaseConnection, DatabaseConnectionRepository>();

//Adds Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Adds API Controllers
builder.Services.AddControllers();

//Adds Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/access-denied";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(30);
        options.AccessDeniedPath = "/access-denied";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

//Adds controllers
app.MapControllers();

//Adds Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(TaskBox.Client._Imports).Assembly);

app.Run();
