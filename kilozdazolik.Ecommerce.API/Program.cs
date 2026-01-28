using kilozdazolik.Ecommerce.API.Data;
using kilozdazolik.Ecommerce.API.Features.Categories;
using kilozdazolik.Ecommerce.API.Features.Products;
using kilozdazolik.Ecommerce.API.Features.Sales;
using kilozdazolik.Ecommerce.API.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISaleService, SaleService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

app.MapControllers();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.Run();
