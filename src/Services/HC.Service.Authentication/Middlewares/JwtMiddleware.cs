using HC.Service.Authentication.Services.IServices;

namespace HC.Service.Authentication.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtTokenGenerator jwtTokenGenerator)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var principal = jwtTokenGenerator.ValidateToken(token);

                if (principal != null)
                {
                    context.User = principal;
                }
            }

            await _next(context);
        }
    }
}