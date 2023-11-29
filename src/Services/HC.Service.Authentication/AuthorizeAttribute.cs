using HC.Foundation.Common;
using HC.Foundation.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static HC.Foundation.Common.Constants.Constants;

namespace HC.Service.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<Role> _roles;

        public AuthorizeAttribute(params Role[] roles)
        {
            _roles = roles ?? [];
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                var isRolePermission = false;
                var user = context.HttpContext.User;

                if (user == null || !user.Identity.IsAuthenticated)
                {
                    response = ApiResponse.GetResponseResult(response, StatusCodes.Status401Unauthorized, Foundation.Common.Constants.Constants.ResponseResult.Description.TOKEN_INVALID);

                    context.Result = new JsonResult(response)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };

                    return;
                }

                if (_roles.Any())
                {
                    foreach (var role in _roles)
                    {
                        var roleName = RoleInfoAttribute.ToName(role);
                        if (user.IsInRole(roleName))
                        {
                            isRolePermission = true;
                            break;
                        }
                    }
                }
                else
                {
                    var roleName = RoleInfoAttribute.ToName(Role.Admin);
                    if (user.IsInRole(roleName))
                    {
                        isRolePermission = true;
                    }
                }

                if (!isRolePermission)
                {
                    response = ApiResponse.GetResponseResult(response, StatusCodes.Status401Unauthorized, Foundation.Common.Constants.Constants.ResponseResult.Description.NO_PERMISSION);

                    context.Result = new JsonResult(response)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
            }
            catch (Exception ex)
            {
                response = ApiResponse.GetResponseResult(response, StatusCodes.Status401Unauthorized, ex.Message);

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };

                return;
            }
        }
    }
}