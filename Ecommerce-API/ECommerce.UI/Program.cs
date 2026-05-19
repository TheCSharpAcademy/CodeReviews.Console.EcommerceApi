using ECommerce.UI.Configuration;
using ECommerce.UI.Interfaces;
using ECommerce.UI.Services;
using ECommerce.UI.UserInterface.AdministratorUi;
using ECommerce.UI.UserInterface.TestingUi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IApiService, ApiService>();
builder.Services.AddTransient<IItemService, ItemService>();
builder.Services.AddTransient<ITagService, TagService>();
builder.Services.AddTransient<ISaleService, SaleService>();
builder.Services.AddTransient<ICartService, CartService>();

builder.Services.AddTransient<IVerificationService, VerificationService>();

builder.Services.AddTransient<ManageProductsUi>();
builder.Services.AddTransient<ManageProductTagsUi>();
builder.Services.AddTransient<ManageSalesUi>();

builder.Services.AddTransient<ProductTestUi>();
builder.Services.AddTransient<CheckoutUi>();

builder.Services.AddTransient<AdministratorUi>();
builder.Services.AddTransient<TestingUi>();

builder.Services.AddHttpClient(ApiSettings.BaseUrl,
    client => client.BaseAddress = new Uri(ApiSettings.BaseUrl));
builder.Services.AddHostedService<Worker>();

builder.Logging.SetMinimumLevel(LogLevel.Warning);

var app = builder.Build();

await app.RunAsync();

internal class Worker : BackgroundService
{
    private readonly AdministratorUi _adminUi;

    public Worker(AdministratorUi adminUi)
    {
        _adminUi = adminUi;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _adminUi.MainMenu();
    }
}