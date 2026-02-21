using _01_Framework.Application;
using _01_Framework.Application.AwsServices;
using _01_Framework.Application.Email;
using _01_Framework.Application.Sms;
using _01_Framework.Application.TusServices;
using _01_Framework.Application.ZarinPal;
using _01_Framework.Hubs;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.Configuration;
using Amazon.S3;
using LogManagement.Infrastructure.Configuration;
using MessageManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using ServiceHost;
using StudyManagement.Infrastructure.Configuration;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;


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

        
        options.Conventions.AuthorizeAreaFolder("Professor", "/", "ProfessorArea");
    });
var connectionString = builder.Configuration.GetConnectionString("BehAmoozDb");

StudyManagementBootstrapper.Configure(services,connectionString); 
MessageManagementBootstrapper.Configure(services,connectionString);
AccountManagementBootstrapper.Configure(services,connectionString);
LogManagementBootstrapper.Configure(services, connectionString); 
services.AddTransient<IAuthHelper, AuthHelper>();
services.AddTransient<IPasswordHasher, PasswordHasher>();
services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
services.AddTransient<ISmsService, SmsService>();
services.AddTransient<IEmailService, EmailService>();
services.AddScoped<IFileManager, FileManager>();
services.AddScoped<IStorageServiceAws, StorageServiceAws>();
services.AddScoped<IStorageServiceTus, StorageServiceTus>();


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
    options.AddPolicy("AdminArea", p => p.RequireRole(Roles.Administrator, Roles.SuperAdministrator));

    options.AddPolicy("ProfessorArea", p => p.RequireRole(Roles.Professor));
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

app.UseRouting();

app.UseAuthentication();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseCookiePolicy();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.MapHub<UploadHub>("/uploadHub");

app.Run();
