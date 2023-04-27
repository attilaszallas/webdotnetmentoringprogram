using Microsoft.EntityFrameworkCore;
using Serilog;
using WebDotNetMentoringProgram.Data;
using WebDotNetMentoringProgram.Filters;
using WebDotNetMentoringProgram.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WebDotNetMentoringProgramContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebDotNetMentoringProgramContext") ?? throw new InvalidOperationException("Connection string 'WebDotNetMentoringProgramContext' not found.")));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<LoggingResponseHeaderFilterService>();

/* Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.File(builder.Configuration["Logging:LogFilePath"]));
*/

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

app.Logger.LogInformation("Use Routing");
app.UseRouting();

app.Logger.LogInformation("Use Authorization");
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

string contentRootPath = app.Environment.ContentRootPath.ToString();
string? configurationAllowedHosts = app.Configuration.GetSection("AllowedHosts")?.Value?.ToString();

app.Logger.LogInformation("Application Startup", contentRootPath);
app.Logger.LogInformation($"Additional information: application location - folder path: {contentRootPath}");
app.Logger.LogInformation($"Additional information: current configuration values: {configurationAllowedHosts}");

app.Use(async (context, next) =>
{
    await next(context);

    if (context.Response.ContentType == "image/bmp")
    {
        var originalBody = context.Response.Body;

        try
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await next(context);

                memoryStream.Position = 0;

                bytes = memoryStream.ToArray();
            }

            using(var fileStream = new FileStream("C:\\Users\\kryst\\Desktop\\test.bmp", FileMode.Create, System.IO.FileAccess.Write))
            {
                fileStream.Write(bytes, 0, bytes.Length);
            }

        }
        finally
        {
            context.Response.Body = originalBody;
        }

        var contextRequestPath = context.Request.Path;

        RouteData routetData = context.GetRouteData();

        var routeValue = context.GetRouteValue;
    }
});

app.Run();
