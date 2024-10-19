using clout.Interface;
using clout.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IUserIdGeneratorService _userIdGeneratorService;
        private readonly IUserRepository _userRepository;

        public AccountController(IAuthService authService, IEmailService emailService, IUserIdGeneratorService userIdGeneratorService)
        {
            _authService = authService;
            _emailService = emailService;
            _userIdGeneratorService = userIdGeneratorService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.LoginAsync(model);
            if (result == null)
                return Unauthorized();
            return Ok(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegisterModel model)
        {
            if (model == null)
            {
                return BadRequest("it is from post-signup");
            }

            var userId = _userIdGeneratorService.GenerateUserId();
            var result = await _authService.SignUpAsync(model);

            if (result == null)
            {
                return BadRequest("SignupAsync() didn't work");
            }

            if (result != "User created successfully!")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var result = await _authService.ForgotPasswordAsync(model);
            if (!result)
                return BadRequest();
            return Ok(new { Message = "Password reset link has been sent to your email" });
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var result = await _authService.ResetPasswordAsync(model);
            if (!result)
                return BadRequest();
            return Ok(new { Message = "Password has been reset successfully" });
        }
    }
}
