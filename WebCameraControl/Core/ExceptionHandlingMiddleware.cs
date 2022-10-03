namespace WebCameraControl.Core
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 404)
                {
                    httpContext.Response.Redirect("/");
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string contentScript = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
                    <script>
                        alert(`{exception.Message}`);
                        window.location.href = ""/"";
                    </script>
                </head>
                </html>
            ";

            await context.Response.WriteAsync(contentScript);
        }
    }
}
