using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Data.Configuration;
using ECommerce.Api.TerrenceLGee.Repositories;
using ECommerce.Api.TerrenceLGee.Services;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
Directory.CreateDirectory(loggingDirectory);
var filePath = Path.Combine(loggingDirectory, "app-.txt");
var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(filePath, rollingInterval: RollingInterval.Day, outputTemplate: outputTemplate)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ECommerceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .ConfigureWarnings(warnings =>
    {
        warnings.Ignore(RelationalEventId.PendingModelChangesWarning);
    });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ECommerceDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

var authConfiguration = new AuthConfiguration();
builder.Configuration.GetSection(authConfiguration.Section)
    .Bind(authConfiguration);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = authConfiguration.Issuer,
            ValidAudience = authConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfiguration.Key))
        };
    });

builder.Services.Configure<AuthConfiguration>(builder.Configuration.GetSection(authConfiguration.Section));

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISaleService, SaleService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await dbContext.Database.EnsureCreatedAsync();

    await DatabaseSeeder.SeedCategoriesAsync(dbContext);
    await DatabaseSeeder.SeedProductsAsync(dbContext);
    await DatabaseSeeder.SeedUsersAsync(dbContext, userManager, roleManager);
}

app.Run();
