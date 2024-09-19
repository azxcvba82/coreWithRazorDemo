namespace WebApplicationDemo.Model
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtBlacklistService _jwtBlacklistService;

        public TokenBlacklistMiddleware(RequestDelegate next, JwtBlacklistService jwtBlacklistService)
        {
            _next = next;
            _jwtBlacklistService = jwtBlacklistService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token) && _jwtBlacklistService.IsTokenBlacklisted(token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next(context);
        }
    }
}
