using clout.Entity;
using clout.Model;

namespace clout.Interface
{
    public interface IAuthService
    {
        Task<User> LoginAsync(LoginModel model);
        Task<string> SignUpAsync(RegisterModel model);
        Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    }
}
