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
                _logger.LogError($"Something went wrong: {ex.Message}. + ERROR CODE:{400}", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogError($"UNAUTHORIZED: {ex.Message}. + ERROR CODE:{403}", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.Forbidden);
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}. + ERROR CODE:{404}", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.NotFound);

            }
            catch (UserAlreadyExistsException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}. +  ERROR CODE:{400}", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}. +  ERROR CODE:{401}", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.Unauthorized);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}. +  ERROR CODE:{400}", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (UserUnfollowException ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}.+  ERROR CODE:{400}", ex);

                await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex.Message}. +  ERROR CODE:{500}", ex);

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
