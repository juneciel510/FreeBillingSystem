
using FluentValidation;
using FreeBilling.Data.Entities;
using FreeBilling.Web.Apis;
using FreeBilling.Web.Data;
using FreeBilling.Web.Services;
using FreeBilling.Web.Validators;
using Mapster;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FreeBilling.Web.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BillingDb") 
    ?? throw new InvalidOperationException("Connection string 'BillingDb' not found.");

IConfigurationBuilder configBuilder = builder.Configuration;
configBuilder.Sources.Clear();
configBuilder.AddJsonFile("appsettings.json")
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddUserSecrets(Assembly.GetExecutingAssembly())
    .AddEnvironmentVariables()
    .AddCommandLine(args);

builder.Services.AddDbContext<BillingContext>();

builder.Services.AddIdentityApiEndpoints<TimeBillUser>(options => 
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<BillingContext>();

builder.Services.AddAuthentication().AddBearerToken();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("api", cfg => 
    {
        cfg.RequireAuthenticatedUser();
        cfg.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ApiPolicy", cfg =>
    {
        cfg.RequireAuthenticatedUser();
        cfg.AddAuthenticationSchemes(IdentityConstants.BearerScheme);
    });

//builder.Services.AddAuthorization(cfg =>
//{
//    cfg.AddPolicy("ApiPolicy", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
//    });
//});

builder.Services.AddScoped<IBillingRepository, BillingRepository>();

builder.Services.AddRazorPages();
builder.Services.AddTransient<IEmailService, DevTimeEmailService>();
builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<TimeBillModelValidator>();

TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly()!);
var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


//Allow us to serve index.html as the default webpage
app.UseDefaultFiles();

//Allows us to serve files from wwwroot folder
app.UseStaticFiles();

//Add Routing
//app.UseRouting();
app.UseAuthentication();

//Add Authorization
app.UseAuthorization();

app.MapRazorPages();

//app.MapGet("/", () => "Hello World!");
//app.Run(async ctx =>
//{
//    await ctx.Response.WriteAsync("<html><body><h1> Welcome to FreeBilling</h1></body></html>");
//});

TimeBillsApi.Register(app);

app.MapControllers();

app.MapGroup("api/auth").MapIdentityApi<TimeBillUser>();

app.Run();
