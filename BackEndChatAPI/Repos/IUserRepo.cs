using BackEndChatAPI.Models;

namespace BackEndChatAPI.Repos
{
    public interface IUserRepo
    {
        List<Users> GetAllUsers();
    }
}