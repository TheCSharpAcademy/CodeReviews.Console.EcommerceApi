using EcommerceApi.Context;
using EcommerceApi.Services;
using EcommerceApi.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<EcommerceDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
builder.Services.Configure<PaginationSettings>(
    builder.Configuration.GetSection("PaginationSettings"));
builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<ProductsService>();
builder.Services.AddScoped<SalesService>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
