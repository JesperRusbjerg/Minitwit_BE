using Minitwit_BE.Domain.Exceptions;
using System.Net;

namespace Minitwit_BE.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (MessageNotFoundException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}.", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}.", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}.", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.Unauthorized);
            }
            catch (UserUnfollowException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}.", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}.", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string message, HttpStatusCode httpStatusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) httpStatusCode;

            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
