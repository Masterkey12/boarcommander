using Core.BoardCommander.Exceptions;

namespace BardCommander.Api.Middlewares
{
    public class ErrorHandlerMidleware
    {
        private readonly RequestDelegate _next;
        public ErrorHandlerMidleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
               await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is BaseApiException exception)
                {
                    context.Response.StatusCode = exception.StatusCode;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        ExceptionType = ex.GetType().Name,
                        Message = exception.Message,
                        StackTrace = exception.StackTrace
                    }) ;
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        ExceptionType = ex.GetType().Name,
                        StackTrace = ex.StackTrace
                    });
                }
            }
        }
    }

    public static class RequestErrorHandlerMidleware
    {
        public static IApplicationBuilder UseErrorHandler(
        this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMidleware>();
        }
    }
}
