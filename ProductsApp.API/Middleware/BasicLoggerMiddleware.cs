namespace ProductsApp.API.Middleware
{
    public class BasicLoggerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // In a real world app, we would use a proper logging framework and log more details (e.g. request path, method, headers, etc.)
            Console.WriteLine($"Middleware executing... ");

            await _next(context);

            Console.WriteLine($"Middleware executed... ");
        }
    }
}
