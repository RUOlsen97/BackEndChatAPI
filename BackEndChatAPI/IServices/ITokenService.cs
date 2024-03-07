using static CodeFirstDb.ChatWebAppContext;

namespace BackEndChatAPI.IServices
{
    public interface ITokenService
    {
        string GenerateJwtToken(Models.User user);
        string? ValidateJwtToken(string token);

    }
}
