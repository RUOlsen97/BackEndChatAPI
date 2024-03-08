using static CodeFirstDb.ChatWebAppContext;

namespace BackEndChatAPI.IServices
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(Models.User user);
        Task<string?> ValidateJwtToken(string token);

    }
}
