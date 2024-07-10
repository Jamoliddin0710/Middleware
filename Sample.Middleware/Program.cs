using Sample.Middleware;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseAuthMiddleware();

app.Run();