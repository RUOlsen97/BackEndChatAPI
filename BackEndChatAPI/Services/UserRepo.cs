using BackEndChatAPI.IServices;
using BackEndChatAPI.Models;
using CodeFirstDb;
namespace BackEndChatAPI.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly ChatWebAppContext _context;

        public UserRepo( ChatWebAppContext context) 
        {
            _context = context;
        }
    }
}
