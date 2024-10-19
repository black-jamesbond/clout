using clout.Entity;
using clout.Model;

namespace clout.Interface
{
    public interface IUserRepository
    {
        Task<User> RegisterAsync(RegisterModel model);
        Task<User> LoginAsync(LoginModel model);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
