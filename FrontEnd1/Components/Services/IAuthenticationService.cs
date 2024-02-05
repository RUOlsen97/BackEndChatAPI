using FrontEnd1.Components.Models;

namespace FrontEnd1.Components.Services
{
    public interface IAuthenticationService
    {
        event Action<string?>? LoginChange;

        Task<bool> Authenticate(bool authenticated);
        ValueTask<string> GetJwtAsync();
        Task<DateTime> LoginAsync(string email, string password);
        Task OnAfterRenderAsync();
        string GetUsername(string token);
        Task LogoutAsync();
    }
}