using Newtonsoft.Json;

namespace WebCameraControl.Core
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next)
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
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                string json = JsonConvert.SerializeObject(ex.Message);

                await context.Response.WriteAsync(json);
            }
        }
    }

    public static class ErrorWrappingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorWrappingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
