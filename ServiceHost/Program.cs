using _01_Framework.Application;
using _01_Framework.Application.Email;
using _01_Framework.Application.Sms;
using _01_Framework.Application.ZarinPal;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.Configuration;
using MessageManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ServiceHost;
using StudyManagement.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddHttpContextAccessor();

services.AddRazorPages()
    .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeAreaFolder("Administration", "/", "AdminArea");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Course", "Course");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Message", "Message");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Accounts", "Account");
    }); ;
var connectionString = builder.Configuration.GetConnectionString("BehAmoozDb");

StudyManagementBootstrapper.Configure(services,connectionString); 
MessageManagementBootstrapper.Configure(services,connectionString);
AccountManagementBootstrapper.Configure(services,connectionString);
services.AddTransient<IPasswordHasher, PasswordHasher>();
services.AddTransient<IAuthHelper, AuthHelper>();
services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
services.AddTransient<ISmsService, SmsService>();
services.AddTransient<IEmailService, EmailService>();



services.Configure<CookieTempDataProviderOptions>(options => {
    options.Cookie.IsEssential = true;
});

services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = new PathString("/Login");
        o.LogoutPath = new PathString("/Login");
        o.AccessDeniedPath = new PathString("/AccessDenied");
    });

services.AddAuthorization(options =>
{
    options.AddPolicy("AdminArea",
        builder => builder.RequireRole(new List<string> { Roles.Administrator, Roles.Professor }));

    options.AddPolicy("Course",
        builder => builder.RequireRole(new List<string> { Roles.Administrator, Roles.Professor }));

    options.AddPolicy("Message",
        builder => builder.RequireRole(new List<string> { Roles.Administrator }));

    options.AddPolicy("Account",
        builder => builder.RequireRole(new List<string> { Roles.Administrator, Roles.Professor }));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

app.Run();
