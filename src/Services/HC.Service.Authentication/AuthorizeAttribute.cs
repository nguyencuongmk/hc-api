using Azure;
using HC.Foundation.Core.Constants;
using HC.Foundation.Cormmon;
using HC.Service.Authentication.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using System.IdentityModel.Tokens.Jwt;

namespace HC.Service.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string Roles { get; set; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ApiResponse response;

            try
            {
                var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();

                if (string.IsNullOrEmpty(token))
                {
                    response = ResponseResult(StatusCodes.Status401Unauthorized, Constants.ResponseResult.Description.TOKEN_EMPTY);

                    context.Result = new JsonResult(response)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };

                    return;
                }

                var jst = new JwtSecurityToken(token);

                if (jst == null || jst.ValidFrom > DateTime.UtcNow || jst.ValidTo < DateTime.UtcNow)
                {
                    response = ResponseResult(StatusCodes.Status401Unauthorized, Constants.ResponseResult.Description.TOKEN_EXPIRED);

                    context.Result = new JsonResult(response)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };

                    return;
                }

                var valid = await ValidateToken(context, token);

                if (!valid)
                {
                    response = ResponseResult(StatusCodes.Status401Unauthorized, Constants.ResponseResult.Description.TOKEN_INVALID);

                    context.Result = new JsonResult(response)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };

                    return;
                }
            }
            catch (Exception ex)
            {
                response = ResponseResult(StatusCodes.Status401Unauthorized, ex.Message);

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                return;
            }
        }

        private ApiResponse ResponseResult(int statusCode, string description)
        {
            return new ApiResponse
            {
                Result =
                {
                    StatusCode = statusCode,
                    Message = ReasonPhrases.GetReasonPhrase(statusCode),
                    Description = description
                }
            };
        }

        private async Task<bool> ValidateToken(AuthorizationFilterContext context, string token)
        {
            bool isValidToken = false;
            var userRepository = context.HttpContext.RequestServices.GetService(typeof(IUserRepository)) as IUserRepository;
            if (userRepository != null)
            {
                isValidToken = await userRepository.Verify(token);
            }
            return isValidToken;
        }
    }
}