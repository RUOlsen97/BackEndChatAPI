using BackEndChatAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace BackEndChatAPI.context
{
    public class NewContext : IdentityDbContext<Users>
    {
        public NewContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Messages> Messages { get; set; }
        public DbSet<Users>Users {  get; set; }
    }
}
