using HC.Foundation.Cormmon;
using HC.Service.Authentication.Models.Requests;
using HC.Service.Authentication.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace HC.Service.Authentication.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = new ApiResponse();
            var errorMessage = await _authService.Register(request);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                response = ApiResponse.GetResponseResult(response, StatusCodes.Status400BadRequest, errorMessage);
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}