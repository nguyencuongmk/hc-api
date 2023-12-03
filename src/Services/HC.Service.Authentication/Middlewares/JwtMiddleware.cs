using HC.Foundation.Common.Helpers;
using HC.Service.Authentication.Settings;
using Microsoft.Extensions.Options;

namespace HC.Service.Authentication.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var principal = JwtHelper.ValidateToken(token, _appSettings.JwtSettings.Issuer, _appSettings.JwtSettings.Audience, _appSettings.JwtSettings.Secret);

                if (principal != null)
                {
                    context.User = principal;
                }
            }

            await _next(context);
        }
    }
}