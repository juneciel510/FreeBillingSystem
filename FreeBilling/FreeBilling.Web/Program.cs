
using FreeBilling.Web.Data;
using FreeBilling.Web.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

IConfigurationBuilder configBuilder = builder.Configuration;
configBuilder.Sources.Clear();
configBuilder.AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddDbContext<BillingContext>();

builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


//Allow us to serve index.html as the default webpage
app.UseDefaultFiles();

//Allows us to serve files from wwwroot folder
app.UseStaticFiles();

app.MapRazorPages();

//app.MapGet("/", () => "Hello World!");
//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("<html><body><h1> Welcome to FreeBilling</h1></body></html>");
//});


app.Run();
