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
        public async Task<IActionResult> Login(Login user)
        {
           var token = await _userService.GetAuthenticationToken(user.Username, user.Password);

            if (token == null)
                return Unauthorized("User not found");

            return Ok(token);
        }
    }
}
