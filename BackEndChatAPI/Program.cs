using BackEndChatAPI.IServices;
using BackEndChatAPI.Services;

using CodeFirstDb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackEndChatAPI.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using BackEndChatAPI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using BackEndChatAPI.context;
using System.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using BackEndChatAPI.Repos;
using Microsoft.AspNetCore.Identity.UI.Services;
using BackEndChatAPI.Repos.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Text;


string connectionstring = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-WebApp1-9a471532-62be-40c6-b4d6-afd15a758cec;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    // Identity made Cookie authentication the default.
    // However, we want JWT Bearer Auth to be the default.
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("YourVeryLongAndComplexSecretKeyHere")),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    // Configure the Authority to the expected value for
    // the authentication provider. This ensures the token
    // is appropriately validated.
    options.Authority = "https://localhost:7222"; // TODO: Update URL

    // We have to hook the OnMessageReceived event in order to
    // allow the JWT authentication handler to read the access
    // token from the query string when a WebSocket or 
    // Server-Sent Events request comes in.

    // Sending the access token in the query string is required when using WebSockets or ServerSentEvents
    // due to a limitation in Browser APIs. We restrict it to only calls to the
    // SignalR hub in this code.
    // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
    // for more information about security considerations when using
    // the query string to transmit the access token.
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // If the request is for our hub...
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/hubs/chathub")))
            {
                // Read the token out of the query string
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
}).AddIdentityServerJwt();/*.AddBearerToken(IdentityConstants.BearerScheme)*/;
builder.Services.AddAuthorizationBuilder().AddPolicy("api", p =>
{
    p.RequireAuthenticatedUser();
});

builder.Services.AddDbContext<NewContext>(options =>
options.UseSqlServer(connectionstring));

//builder.Services.AddIdentityApiEndpoints<Users>().AddEntityFrameworkStores<NewContext>();

//builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddScoped<SignInManager<User>>();
builder.Services.AddScoped<UserManager<User>>();

builder.Services.AddScoped<IMessagesRepo, MessagesRepo>();

builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedAccount = false;
})
        .AddEntityFrameworkStores<NewContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

//builder.Services.AddDefaultIdentity<IdentityUser>(options =>
//{
//    options.SignIn.RequireConfirmedAccount = false;
//})
//.AddEntityFrameworkStores<NewContext>();

//builder.Services.AddIdentity<Users, IdentityRole>().AddEntityFrameworkStores<NewContext>().AddDefaultTokenProviders();


builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 0;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddMvc();

builder.Services.TryAddEnumerable(
    ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
        ConfigureJwtBearerOptions>());



var app = builder.Build();

//SeedData.Initialize(app.Services, UserManager<User>(), ).Wait();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapIdentityApi<Users>();

app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
    .RequireAuthorization();

app.MapHub<ChatHub>("/chathub");

app.Run();