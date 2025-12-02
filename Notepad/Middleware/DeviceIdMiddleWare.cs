namespace Notepad.Middleware
{
    public class DeviceIdMiddleWare
    {
        private readonly RequestDelegate _next;
        public DeviceIdMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
          
            if (!context.Request.Cookies.ContainsKey("DeviceId"))
            {
                var deviceId = Guid.NewGuid().ToString();
                context.Response.Cookies.Append("DeviceId", deviceId, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
            }
            await _next(context);
        }
    }
}
