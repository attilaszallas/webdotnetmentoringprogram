using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Data;
using WebDotNetMentoringProgram.Filters;
using WebDotNetMentoringProgram.MiddleWares;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.Repositories;

var AllowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebDotNetMentoringProgramContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebDotNetMentoringProgramContext") ?? throw new InvalidOperationException("Connection string 'WebDotNetMentoringProgramContext' not found.")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                      });
});

builder.Services.AddScoped<LoggingResponseHeaderFilterService>();

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration["Logging:LogFilePath"]));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

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
    // app.UseExceptionHandler("/Home/CustomError");

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Logger.LogInformation("Use Routing");
app.UseRouting();

app.UseCors(AllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

string contentRootPath = app.Environment.ContentRootPath.ToString();
string? configurationAllowedHosts = app.Configuration.GetSection("AllowedHosts")?.Value?.ToString();

app.Logger.LogInformation("Application Startup", contentRootPath);
app.Logger.LogInformation($"Additional information: application location - folder path: {contentRootPath}");
app.Logger.LogInformation($"Additional information: current configuration values: {configurationAllowedHosts}");

app.UseMiddleware<ImageFileCacheMiddleWare>();

app.Run();
