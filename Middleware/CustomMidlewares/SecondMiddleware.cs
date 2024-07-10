namespace Middleware.CustomMidlewares;

public class SecondMiddleware
{
    public readonly RequestDelegate _next;

    public SecondMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var fullName = context.Request.Query["firstname"] + context.Request.Query["lastname"];
        await context.Response.WriteAsync(fullName + "\n");
        await _next(context);
    }
}

public static class SecondMiddlewareExtension
{
    public static IApplicationBuilder UseSecondMiddleware(this IApplicationBuilder builder)
    {
      return  builder.UseMiddleware<SecondMiddleware>();
    }
}