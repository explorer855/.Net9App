using AuthApi.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        public AuthController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Login([FromRoute]UserLoginModel user)
        {
            return Ok();
        }
    }
}
