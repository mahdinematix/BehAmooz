using _01_Framework.Application;
using _01_Framework.Application.AwsServices;
using _01_Framework.Application.Email;
using _01_Framework.Application.Sms;
using _01_Framework.Application.ZarinPal;
using _01_Framework.Hubs;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.Configuration;
using Amazon.S3;
using MessageManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ServiceHost;
using StudyManagement.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddScoped<IAuthHelper, AuthHelper>();
services.AddMemoryCache();
services.AddHttpContextAccessor();
services.AddControllers();
services.AddAWSService<IAmazonS3>();
services.AddSignalR();
services.AddRazorPages()
    .AddMvcOptions(options => options.Filters.Add<SecurityPageFilter>())
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeAreaFolder("Administration", "/", "AdminArea");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Course", "Course");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Message", "Message");
        options.Conventions.AuthorizeAreaFolder("Administration", "/Account", "Account");
    });
var connectionString = builder.Configuration.GetConnectionString("BehAmoozDb");

StudyManagementBootstrapper.Configure(services,connectionString); 
MessageManagementBootstrapper.Configure(services,connectionString);
AccountManagementBootstrapper.Configure(services,connectionString);
services.AddTransient<IPasswordHasher, PasswordHasher>();
services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
services.AddTransient<ISmsService, SmsService>();
services.AddTransient<IEmailService, EmailService>();
services.AddScoped<IFileManager, FileManager>();
services.AddScoped<IStorageService, StorageService>();

services.Configure<CookieTempDataProviderOptions>(options => {
    options.Cookie.IsEssential = true;
});

services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
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

builder.WebHost.ConfigureKestrel(o =>
{
    o.Limits.MaxRequestBodySize = 1_073_741_824;
    o.AddServerHeader = false;
    o.Limits.MaxRequestBodySize = null;

});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_073_741_824;
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartHeadersLengthLimit = int.MaxValue;
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

app.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action}")
    .WithStaticAssets();

app.MapAreaControllerRoute(
    name: "AreaRoute",
    areaName: "Administration",
    pattern: "{area}/{controller}/{action}"
).WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.MapHub<UploadHub>("/uploadHub");

app.Run();
