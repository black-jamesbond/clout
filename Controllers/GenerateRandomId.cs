using clout.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateRandomId : ControllerBase
    {
        private readonly IUserIdGeneratorService _userIdGeneratorService;

        public GenerateRandomId(IUserIdGeneratorService userIdGeneratorService)
        {
            _userIdGeneratorService = userIdGeneratorService;
        }

        [HttpGet("ramdom Id")]
        public Task<ActionResult<string>> GenerateRandomUserId()
        {
            var user_id = _userIdGeneratorService.GenerateUserId();
            return Task.FromResult<ActionResult<string>>(Ok(user_id));
        }
    }
}
