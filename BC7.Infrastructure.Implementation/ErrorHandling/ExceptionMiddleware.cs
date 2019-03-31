using System;
using System.Net;
using System.Threading.Tasks;
using BC7.Infrastructure.CustomExceptions;
using BC7.Infrastructure.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BC7.Infrastructure.Implementation.ErrorHandling
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
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.BadRequest;
            var exceptionType = exception.GetType();

            switch (exception)
            {
                case Exception e when exceptionType == typeof(UnauthorizedAccessException):
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case AccountNotFoundException e when exceptionType == typeof(AccountNotFoundException):
                case ValidationException ex when exceptionType == typeof(ValidationException):
                    statusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case Exception e when exceptionType == typeof(Exception):
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = statusCode,
                Message = $"{exception.Message}"
            }.ToString());
        }
    }
}
