using Serilog.Context;

namespace Minitwit_BE.Api.Middleware
{
    public class LogContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string pathPropertyName;
        
        public LogContextMiddleware(RequestDelegate next)
        {
            _next = next;
            pathPropertyName = "path";
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            using (LogContext.PushProperty(pathPropertyName, httpContext.Request.Path))
            {
                await _next(httpContext);
            }
        }
    }
}
