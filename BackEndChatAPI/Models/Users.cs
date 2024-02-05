using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BackEndChatAPI.Models
{
    public class Users : IdentityUser
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}