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
        //private Users _user;
        //private Users _user;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;

        public UserController(IUserRepo repo, UserManager<Users> userManager, SignInManager<Users> signInManager)
        {
            _repo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            //_user = user;
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
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginViewModel model) 
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null) 
            {
                return Unauthorized(); 
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded) 
            {
                return Ok("Login successful");
            }
            else
            {
                return Unauthorized();
            }

        }

    }
}
