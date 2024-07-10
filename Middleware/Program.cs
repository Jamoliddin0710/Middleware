using Middleware;
using Middleware.CustomMidlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<FirstMiddlewere>();
// Add services to the container.

var app =  builder.Build();

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Word 1");
    await next(context);
});

app.UseWhen(context => context.Request.Query.ContainsKey("username"),
    app => app.Use(async (context, _next) =>
    {
        await context.Response.WriteAsync("Hello user \n");
        await _next(context);
    }));
app.Use(async (context, @delegate) =>
{
    await context.Response.WriteAsync("user not found");
    await @delegate(context);
});

/*app.UseFirstMiddleware();*/
//app.UseMiddleware<FirstMiddlewere>();
app.UseSecondMiddleware();
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Word 2");
   // await next(context);
});



app.Run();