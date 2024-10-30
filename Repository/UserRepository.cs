using clout.Interface;
using clout.Entity;
using clout.Model;
using Microsoft.EntityFrameworkCore;
using clout.DbContexts;


namespace clout.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly clout_DbContexts _context;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        //private readonly ITokenService _tokenService;

        public UserRepository(clout_DbContexts context, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
            //_tokenService = tokenService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.OrderBy(u => u.Username).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> RegisterAsync(RegisterModel model)
        {
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginAsync(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == model.Username);

            if (user == null)
            {
                return null;
            }

            //if (user.Password != model.Password)
            //{
            //    return null;
            //}

            return user;
        }

        public async Task<User> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if (user == null)
            {
                return null;
            }

            //var token = _tokenService.GenerateToken();

            //user.ResetPasswordToken = token;
            //user.ResetPasswordTokenExpires = DateTime.Now.AddHours(1);

            //await _context.SaveChangesAsync();

            //var email = new EmailModel
            //{
            //    To = model.Email,
            //    Subject = "Reset Password",
            //    Message = $"Click <a href='{model.CallbackUrl}?token={token}&email={user.Email}'>here</a> to reset your password."
            //};

            //var callbackUrl = model.CallbackUrl + "?token=" + token + "&email=" + user.Email;

            //_emailService.SendEmailAsync(email);

            return user;
        }

        public async Task<User> ResetPasswordAsync(ResetPasswordModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync();
            return user;
        }

        public Task UpdateUserAsync(User user)
        {
            // Attach the user entity to the context
            _context.Users.Attach(user);

            // Mark the entity as modified
            _context.Entry(user).State = EntityState.Modified;

            // Save changes asynchronously
            _context.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}
