using BackEndChatAPI.Models;

namespace BackEndChatAPI.Repos.Interface
{
    public interface IUserRepo
    {
        List<User> GetAllUsers();
        User Adduser(User newUser);
    }
}