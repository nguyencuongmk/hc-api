using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HC.Service.Authentication
{
    public class MyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _requiredClaims;

        public MyAuthorizeAttribute(params string[] claims)
        {
            _requiredClaims = claims;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var hasAllRequredClaims = _requiredClaims.All(claim => context.HttpContext.User.HasClaim(x => x.Type == claim));
            if (!hasAllRequredClaims)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}