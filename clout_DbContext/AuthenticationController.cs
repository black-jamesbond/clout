using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace clout.clout_DbContext
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public class AuthenticaionRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }

        }

        public class User
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string email { get; set; }

            public User(int userId,
                        string username,
                        string email)
            {
                UserId = userId;
                Username = username;
                this.email = email;
            }
        }

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> AuthenticateUser(AuthenticaionRequestBody authenticaionRequestBody)
        {
            var user = ValidateUserCredentials(
                authenticaionRequestBody.Username,
                authenticaionRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }
            var securityKey = new SymmetricSecurityKey(
                Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("username", user.Username));
            claimsForToken.Add(new Claim("email", user.email));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private User ValidateUserCredentials(string username, string? password)
        {
            //var user = _userRepository.GetUserByNameAsync(username).Result;

            var Auth_user = new User(1, "user.Username", "user.Email");

            return Auth_user;

        }

    }
}
