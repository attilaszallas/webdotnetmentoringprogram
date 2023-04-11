using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebDotNetMentoringProgram.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebDotNetMentoringProgramContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebDotNetMentoringProgramContext") ?? throw new InvalidOperationException("Connection string 'WebDotNetMentoringProgramContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration["Logging:LogFilePath"]));

var app = builder.Build();
app.Logger.LogInformation("Application build is ready.");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Home/CustomError");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.Logger.LogInformation("Use Routing");

app.UseAuthorization();
app.Logger.LogInformation("Use Authorization");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

string contentRootPath = app.Environment.ContentRootPath.ToString();
string? configurationAllowedHosts = app.Configuration.GetSection("AllowedHosts")?.Value?.ToString();

app.Logger.LogInformation("Application Startup", contentRootPath);
app.Logger.LogInformation($"Additional information: application location - folder path: {contentRootPath}");
app.Logger.LogInformation($"Additional information: current configuration values: {configurationAllowedHosts}");

app.Run();
