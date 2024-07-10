using Middleware.CustomMidlewares;

namespace Middleware;

public  static class MiddlewareExtension
{
    public static IApplicationBuilder UseFirstMiddleware(this IApplicationBuilder application)
    {
        return application.UseMiddleware<FirstMiddlewere>();
    }
}