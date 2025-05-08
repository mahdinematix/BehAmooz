using MessageManagement.Infrastructure.Configuration;
using StudyManagement.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddHttpContextAccessor();
services.AddCors(options => options.AddPolicy("MyPolicy", builder =>
    builder
        .WithOrigins("https://localhost:44319")
        .AllowAnyHeader()
        .AllowAnyMethod()));
services.AddRazorPages();
var connectionString = builder.Configuration.GetConnectionString("BehAmoozDb");

StudyManagementBootstrapper.Configuration(services,connectionString); 
MessageManagementBootstrapper.Configuration(services,connectionString);

var app = builder.Build();

// Configuration the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseCors("MyPolicy");
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
