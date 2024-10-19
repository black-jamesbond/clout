using clout.Entity;
using clout.Interface;
using clout.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace clout.Services
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public AuthService(IEmailService emailService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<User> LoginAsync(LoginModel model)
        {
            var user = await _userRepository.LoginAsync(model);
            if (user == null)
            {
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

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordModel model)
        {
            return true;
        }

        private string GenerateJwtToken(User user)
        {
            // Implementation for generating JWT token
            return "generated_jwt_token";
        }
    }
}
