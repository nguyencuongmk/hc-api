using Microsoft.AspNetCore.Mvc;

namespace HC.Service.Authentication.Controllers
{
    [Authorize(Roles = "admin")]
    public class AuthController : ControllerBase
    {
        [HttpGet("s")]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}