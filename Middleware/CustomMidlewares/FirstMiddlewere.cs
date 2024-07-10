
namespace Middleware.CustomMidlewares
{
    public class FirstMiddlewere : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
           await context.Response.WriteAsync("Middleware start");
           await next(context);
           await context.Response.WriteAsync("Middleware finished");
        }
    }
}
