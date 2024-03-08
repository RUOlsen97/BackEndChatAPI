using BackEndChatAPI.Controllers;
using BackEndChatAPI.IServices;
using BackEndChatAPI.Models;
using CodeFirstDb;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackEndChatAPI.context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BackEndChatAPI.Services
{
    public class TokenService : ITokenService
    {
        private UserManager<User> _userManager;
        private RoleManager<User> _roleManager;

        public TokenService(NewContext context, UserManager<Models.User> userManager, RoleManager<User> roleManager = null, TokenHandler tokenHandler = null)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public Models.User getUser = new Models.User();
        private readonly NewContext _context;

        
        public async Task<string> GenerateJwtToken(Models.User user)
        {   
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourVeryLongAndComplexSecretKeyHere"); // Replace with your secret key

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string?> ValidateJwtToken(string token)
        {
            if (token == null)
                return "";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourVeryLongAndComplexSecretKeyHere");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userid = jwtToken.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;

                var user = await _userManager.FindByIdAsync(userid);
                if (user == null)
                {
                    return "";
                }
                var roles = await _userManager.GetRolesAsync(user);

                return roles[0];
            }
            catch
            {
                return "";
            }
        }
    }
}
