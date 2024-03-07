using BackEndChatAPI.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BackEndChatAPI.Controllers
{
    public class TokenController : Controller
    {
        private ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpGet("RoleIssuer")]
        public async Task<ActionResult> IssueRoleOnValidToken(string token)
        {
            string role = _tokenService.ValidateJwtToken(token);
            if (role == null)
            {
                return BadRequest();
            }
            return Ok(role);
        }
    }
}
