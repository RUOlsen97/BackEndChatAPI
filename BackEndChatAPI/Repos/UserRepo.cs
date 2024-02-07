using BackEndChatAPI.context;
using BackEndChatAPI.Models;

namespace BackEndChatAPI.Repos
{
    public class UserRepo : IUserRepo
    {
        private NewContext _context;

        public UserRepo(NewContext context)
        {
            _context = context;
        }
        public List<Users> GetAllUsers()
        {
            return _context.Set<Users>().ToList();
        }
    }
}
