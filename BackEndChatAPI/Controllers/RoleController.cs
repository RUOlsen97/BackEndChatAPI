using BackEndChatAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEndChatAPI.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<IdentityRole> rolemanager, UserManager<User> userManager)
        {
            _roleManager = rolemanager;
            _userManager = userManager;
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(string role, string roleid)
        {
            var roleExist = await _roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {

                var roleResult = await _roleManager.CreateAsync(new IdentityRole { Id = roleid, Name = role });
                if (!roleResult.Succeeded)
                {
                    // Handle errors
                    return BadRequest();
                }
            }
            return Ok();
        }

        [HttpPost("AssignRoleToUser")]
        public async Task<ActionResult> AssignAdminToUser(LoginViewModel model)
        {
            var findUser = await _userManager.FindByEmailAsync(model.UserName);
            if (findUser != null)
            {
                var role = "admin";
                var UserRole = await _userManager.AddToRoleAsync(findUser, role);
                if (UserRole.Succeeded)
                {
                    return Ok();
                }
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
