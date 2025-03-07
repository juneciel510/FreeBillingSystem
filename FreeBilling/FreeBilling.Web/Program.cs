var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
app.Run(async ctx =>
{
    await ctx.Response.WriteAsync("<html><body><h1> Welcome to FreeBilling</h1></body></html>");
});


app.Run();
