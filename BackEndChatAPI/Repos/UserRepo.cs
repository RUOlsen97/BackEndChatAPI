using BackEndChatAPI.context;
using BackEndChatAPI.Models;
using BackEndChatAPI.Repos.Interface;

namespace BackEndChatAPI.Repos
{
    public class UserRepo : IUserRepo
    {
        private NewContext _context;

        public UserRepo(NewContext context)
        {
            _context = context;
        }
        public List<User> GetAllUsers()
        {
            return _context.Set<User>().ToList();
        }
        public User Adduser(User newUser)
        {
            _context.User.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }
    }
}
