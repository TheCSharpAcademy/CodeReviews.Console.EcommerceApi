using AutoMapper;
using EcommerceAPI.Ledana.Data;
using EcommerceAPI.Ledana.Interfaces;
using EcommerceAPI.Ledana.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//adding the dbcontext
var connectionString = builder.Configuration.GetConnectionString("ECommerceDb")
                        ?? throw new InvalidOperationException("Connection string 'ECommerceDb' not found!");

builder.Services.AddDbContext<ProductContext>(options =>
                        options.UseSqlServer(connectionString));

//adding the product service
builder.Services.AddScoped<IProductService, ProductService>();
//adding the category service
builder.Services.AddScoped<ICategoryService, CategoryService>();

//adding the sale service
builder.Services.AddScoped<ISaleService, SaleService>();

//configuring json serializer to handle cycles
builder.Services.AddControllers().AddJsonOptions(opt =>
    opt.JsonSerializerOptions.ReferenceHandler =
    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

//add the mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();

//adding swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
