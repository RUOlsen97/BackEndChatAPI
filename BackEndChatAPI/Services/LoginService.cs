using BackEndChatAPI.Controllers;
using BackEndChatAPI.IServices;
using BackEndChatAPI.Models;
using CodeFirstDb;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;
using System.Data.Common;

namespace BackEndChatAPI.Services
{
    public class LoginService : ILoginService
    {
        public LoginService(ChatWebAppContext context)
        {
            this.context = context;
        }
        public Users getUser = new Users();
        private readonly ChatWebAppContext context;

        public bool authenticateUser(string userName, string password)
        {
            var query = from p in context.users where p.Username == userName && p.Password == password select p.userid;
            if (query.Any())
            {
                return true;
            }
            else return false;
        }
    }
}
