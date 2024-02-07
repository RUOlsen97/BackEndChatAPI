using Microsoft.AspNetCore.Mvc;
using CodeFirstDb;
using BackEndChatAPI.Models;
using BackEndChatAPI.Services;
using BackEndChatAPI.IServices;
using BackEndChatAPI.context;
using BackEndChatAPI.Models;
using BackEndChatAPI.Repos;
using Microsoft.AspNetCore.Identity;

namespace BackEndChatAPI.Controllers
{
    [Route("api/[controller]")]
    //URI: api/users
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepo _repo;
        private NewContext _context;
        private ILoginService _loginService;
        private readonly UserManager<Users> _userManager;

        public UserController(IUserRepo repo, UserManager<Users> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
