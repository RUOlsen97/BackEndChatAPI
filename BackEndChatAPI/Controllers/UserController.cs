using Microsoft.AspNetCore.Mvc;
using CodeFirstDb;
using BackEndChatAPI.Models;
using BackEndChatAPI.Services;
using BackEndChatAPI.IServices;

namespace BackEndChatAPI.Controllers
{
        [Route("api/[controller]")]
        //URI: api/users
        [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _repo;
        private ChatWebAppContext _context;
        private ILoginService _loginService;

        public UserController(IUserRepo repo)
        {
            _repo = repo;
        }
    }
}
