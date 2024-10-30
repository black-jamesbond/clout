using clout.Entity;
using clout.Model;

namespace clout.Interface
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginModel model);
        Task<string> SignUpAsync(RegisterModel model);
        Task<User> ResetPasswordAsync(ResetPasswordModel model);
        Task<string> ForgotPasswordAsync(string email);
    }
}
