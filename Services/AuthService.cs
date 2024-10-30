using clout.Entity;
using clout.Interface;
using clout.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace clout.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IEmailService emailService, IUserRepository userRepository, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = GenerateJwtToken(await _userRepository.LoginAsync(model));

            if(user == null)
            {
                _logger.LogError("User not found");
                return null;
            }

            return user;
        }

        public async Task<string> SignUpAsync(RegisterModel model)
        {
            var user = new User { Username = model.Username, Email = model.Email, Password = model.Password };

            var user_1 =  _userRepository.RegisterAsync(model);
            
            if (user_1 == null)
            {
                return null;
            }


            return "User created successfully!";


        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return "there is no user like that";
            }

            EmailModel emailModel = new EmailModel()
            {
                To = user.Email,
                Subject = "Password Reset",
                Message = "Click here to reset your password"
            };
            //var email = emailModel;

            _emailService.SendEmailAsync(emailModel);

            return "An email has been sent to your email address. Please check your email to reset your password.";

        }

        public async Task<User> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = _userRepository.GetUserByEmailAsync(model.Email).Result;

            user.Password = model.NewPassword;

            await _userRepository.UpdateUserAsync(user);

            return user;
        }

        private string GenerateJwtToken(User user)
        {
            // Implementation for generating JWT token
            if (user == null)
            {
                return null;
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

            return tokenToReturn;

        }
    }
}
