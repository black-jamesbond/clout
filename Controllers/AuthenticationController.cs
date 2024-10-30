using clout.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace clout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthenticationController> _logger;
        public class AuthenticaionRequestBody
        {
            public string? Username { get; set; }
            public string? Password { get; set; }

        }

        public class User
        {
            //public int UserId { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }


            public User(
                        string username,
                        string email,
                        string password)
            {
                //UserId = userId;
                Username = username;
                Email = email;
                Password = password;
            }
        }

        public AuthenticationController(IConfiguration configuration, IUserRepository userRepository, ILogger<AuthenticationController> logger)
        {
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
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
            //claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("username", user.Username));
            claimsForToken.Add(new Claim("email", user.Email));
            claimsForToken.Add(new Claim("password", user.Password));

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
            var user = _userRepository.GetUserByUsernameAsync(username).Result;

            if (user.Password != password)
            {
                _logger.LogInformation("Invalid password");
                return null;
            }
            //Authorized users
            var Auth_user = new User(user.Username, user.Email, user.Password);

            return Auth_user;

        }

    }
}
