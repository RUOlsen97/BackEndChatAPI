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
        public async Task<ActionResult> ValidateToken(string token)
        {
            string? role = await _tokenService.ValidateJwtToken(token);
            if (role == "")
            {
                return BadRequest();
            }

            return Ok(role);
        }
    }
}
