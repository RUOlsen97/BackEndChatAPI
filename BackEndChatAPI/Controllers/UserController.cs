using Microsoft.AspNetCore.Mvc;
using CodeFirstDb;
using BackEndChatAPI.Models;
using BackEndChatAPI.Services;
using BackEndChatAPI.IServices;
using BackEndChatAPI.context;
using BackEndChatAPI.Models;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol;
using System.Text.Json;
using BackEndChatAPI.Repos.Interface;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using AutoMapper;

namespace BackEndChatAPI.Controllers
{
    [Route("api/[controller]")]
    //URI: api/users
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepo _repo;
        private NewContext _context;
        private ITokenService _loginService;
        //private Users _user;
        //private Users _user;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(IUserRepo repo, UserManager<User> userManager, SignInManager<User> signInManager, ITokenService loginService, RoleManager<IdentityRole> roleManager)
        {
            _repo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            _loginService = loginService;
            _roleManager = roleManager;

            //_user = user;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
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
            User user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var token = _loginService.GenerateJwtToken(user);
                var loginSucessData = new
                {
                    win = "LoginSuccessful",
                    token = token,
                };
                var json = JsonSerializer.Serialize(loginSucessData);

                return Ok($"{json}");
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult>RegisterAsync(LoginViewModel usermodel)
        {
            
            var find = await _userManager.FindByEmailAsync(usermodel.UserName);
            if (find == null)
            {
                User user = new User
                {
                    UserName = usermodel.UserName
                };
                var result = await _userManager.CreateAsync(user);
                var setPassword = await _userManager.AddPasswordAsync(user, usermodel.Password);
                if (result.Succeeded && setPassword.Succeeded)
                {
                    return Ok();
                }
                else { return BadRequest(); }
            }
            else { return BadRequest(); }
        }
    }
}
