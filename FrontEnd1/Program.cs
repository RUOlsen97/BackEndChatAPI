using Blazored.SessionStorage;
using FrontEnd1.Components;
using FrontEnd1.Components.Account;
using FrontEnd1.Components.Handlers;
using FrontEnd1.Components.Services;
using FrontEnd1.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using FrontEnd1.Components.Account;
using BackEndChatAPI.Services;
using Microsoft.AspNetCore.Authorization;
using BackEndChatAPI.Repos;
//using FrontEnd1.Components.Requirements;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();
builder.Services.TryAddEnumerable(
    ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
        ConfigureJwtBearerOptions>());

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddOptions();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          new[] { "application/octet-stream" });
});

builder.Services.AddScoped<IMessagesRepo, MessagesRepo>();

builder.Services.AddScoped<AuthenticationHandler>();

builder.Services.AddHttpClient("ServerApi").ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7222" ?? "")).AddHttpMessageHandler<AuthenticationHandler>();
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("IsAuthenticatedPolicy", policy =>
//    {
//        policy.Requirements.Add();
//    });
//});
builder.Services.AddAuthorizationBuilder();
builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<IAuthorizationHandler, LoggedInAuthorizationHandler>();

builder.Services.AddScoped<IdentityRedirectManager>();
//builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

//builder.Services.AddScoped<AuthenticationStateProvider,
//    CustomAuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();

builder.Services
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddScoped<FrontEnd1.Components.Services.IAuthenticationService, FrontEnd1.Components.Services.AuthenticationService>();

//builder.Services.AddAuthorization(options =>
//      options.AddPolicy("LoggedIn",
//      policy => policy.Requirements.Add(new LoggedInRequirement(true))));

builder.Services.AddBlazoredSessionStorage();

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAntiforgery();
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<ChatHub>("/chathub");

app.Run();
