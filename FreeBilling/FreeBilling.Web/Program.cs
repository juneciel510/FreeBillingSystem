
using FreeBilling.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


//Allow us to serve index.html as the default webpage
//app.UseDefaultFiles();

////Allows us to serve files from wwwroot folder
//app.UseStaticFiles();

app.MapRazorPages();

//app.MapGet("/", () => "Hello World!");
//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("<html><body><h1> Welcome to FreeBilling</h1></body></html>");
//});


app.Run();
