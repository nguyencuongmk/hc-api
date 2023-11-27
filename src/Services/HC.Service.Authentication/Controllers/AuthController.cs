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

        /// <summary>
        /// Register an account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Admin assign Role to User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        //[Authorize(Roles = "Admin")]
        [HttpPost("assignment-role")]
        public async Task<IActionResult> RoleAssignment(string email, string roleName)
        {
            var response = new ApiResponse();
            var errorMessage = await _authService.AssignRole(email, roleName);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                response = ApiResponse.GetResponseResult(response, StatusCodes.Status400BadRequest, errorMessage);
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = new ApiResponse();
            var loginResponse = await _authService.Login(request);

            if (!string.IsNullOrEmpty(loginResponse.Item2))
            {
                response = ApiResponse.GetResponseResult(response, StatusCodes.Status400BadRequest, loginResponse.Item2);
                return BadRequest(response);
            }

            response.Data = loginResponse.Item1;

            return Ok(response);
        }
    }
}