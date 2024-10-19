using clout.Entity;
using clout.Interface;
using Microsoft.AspNetCore.Mvc;

namespace clout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            if (users == null)
            {
                return BadRequest();
            }
            return Ok(users);
        }
    }
}
