using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Sample.Middleware;

public class AuthMiddleware
{
    public RequestDelegate _next { get; set; }

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if(context.Request.Method == HttpMethods.Get)
        {   context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync("No response");
            await _next(context);
        }
        else if (context.Request.Method != HttpMethods.Post)
        {
            await _next(context);
        }
        
        var body = context.Request.Body;
        using (var reader = new StreamReader(body))
        {
            var result = await reader.ReadToEndAsync();
            Dictionary<string, StringValues> queryDict = QueryHelpers.ParseQuery(result);
            var user = new User();
            user.Email = queryDict.ContainsKey("Email") ? queryDict["Email"][0] : null;
            user.Password = queryDict.ContainsKey("Password") ? queryDict["Password"][0] : null;
    
            if (string.IsNullOrWhiteSpace(user?.Password) || !string.IsNullOrWhiteSpace(user?.Email))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync($"Invalid input for '{(!string.IsNullOrWhiteSpace(user.Email) ? nameof(user.Email): string.IsNullOrWhiteSpace(nameof(user.Password)) )}'");
            }

            else if (string.IsNullOrWhiteSpace(user?.Email) && !string.IsNullOrWhiteSpace(user?.Password))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync($"Invalid for '{nameof(user.Email)}'");
            }
            
            else if (user.Email == ValidData.Email && user.Password == ValidData.Password)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync($"Succesful Login");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid login");
            }

            await _next(context);
        }
    }
}

public static class AuthExtension
{
    public static IApplicationBuilder UseAuthMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}