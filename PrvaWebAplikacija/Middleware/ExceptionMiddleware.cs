using Microsoft.EntityFrameworkCore;
using PrvaWebAplikacija.Common;


namespace PrvaWebAplikacija.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await WriteAsync(context, ex.Message, StatusCodes.Status404NotFound);
            }
            catch (BusinessRuleException ex)
            {
                await WriteAsync(context, ex.Message, StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await WriteAsync(context, "An unexpected error occurred.", StatusCodes.Status500InternalServerError);
            }
            finally
            {
                _logger.LogInformation("{Method} {Path} finished with {StatusCode}.",
                    context.Request.Method, context.Request.Path, context.Response.StatusCode);
            }
        }
        private static Task WriteAsync(HttpContext context, string message, int statusCode)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "text/plain; charset=utf-8"; // so every client decodes č, ć, ž correctly
            return context.Response.WriteAsync(message);
        }
    }
}