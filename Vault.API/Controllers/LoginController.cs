using Microsoft.AspNetCore.Mvc;
using Vault.API.Authentication;

namespace Vault.API.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
           var token = await _userService.GetAuthenticationToken(userName, password);

            if (token == null)
                return Unauthorized("User not found");

            return Ok(token);
        }
    }
}
