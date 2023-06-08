using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Serilog;
using WebDotNetMentoringProgram.Abstractions;
using WebDotNetMentoringProgram.Data;
using WebDotNetMentoringProgram.Filters;
using WebDotNetMentoringProgram.MiddleWares;
using WebDotNetMentoringProgram.Models;
using WebDotNetMentoringProgram.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var AllowSpecificOrigins = "_allowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<WebDotNetMentoringProgramContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebDotNetMentoringProgramContext") ?? throw new InvalidOperationException("Connection string 'WebDotNetMentoringProgramContext' not found.")));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(x => x.LoginPath = "/account/login")
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"), OpenIdConnectDefaults.AuthenticationScheme, "ADCookies");

builder.Services.AddDefaultIdentity<IdentityUser>(
    options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

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

CreateAdminUser(app);

app.Run();

static void CreateAdminUser(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roleName = "Administrator";
        IdentityResult result;

        var roleExist = roleManager.RoleExistsAsync(roleName).Result;
        if (!roleExist)
        {
            result = roleManager.CreateAsync(new IdentityRole(roleName)).Result;
            if (result.Succeeded)
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var admin = userManager.FindByEmailAsync(config["AdminCredentials:Email"]).Result;

                if (admin == null)
                {
                    admin = new IdentityUser()
                    {
                        UserName = config["AdminCredentials:Email"],
                        Email = config["AdminCredentials:Email"],
                        EmailConfirmed = true
                    };

                    result = userManager.CreateAsync(admin, config["AdminCredentials:Password"]).Result;
                    if (result.Succeeded)
                    {
                        result = userManager.AddToRoleAsync(admin, roleName).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception("Error adding administrator");
                        }
                    }
                }
            }
        }
    }
}
