using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Services;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Handlers;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;
using ECommerce.AvaloniaClient.TerrenceLGee.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();

        LoggingSetup();

        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        services.AddSingleton<IAuthTokenHolder, AuthTokenHolder>();
        services.AddTransient<AuthHeaderHandler>();

        services.AddHttpClient("client", c =>
        {
            c.BaseAddress = new Uri(Urls.BaseUrl);
            c.DefaultRequestHeaders.Accept.Add(new("application/json"));
        })
            .AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        services.AddTransient<WelcomePageViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<RegistrationViewModel>();
        services.AddTransient<PasswordResetViewModel>();

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ICategoryService, CategoryService>();
        services.AddSingleton<ICustomerService, CustomerService>();
        services.AddSingleton<IProductService, ProductService>();
        services.AddSingleton<IAddressService, AddressService>();
        services.AddSingleton<ISaleService, SaleService>();
        services.AddSingleton<IShoppingCartService, ShoppingCartService>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MainUserViewModel>();

        services.AddTransient<HomeScreenViewModel>();

        services.AddTransient<CategoryOperationsViewModel>();
        services.AddTransient<AddCategoryViewModel>();
        services.AddTransient<DisplayAddedCategoryViewModel>();
        services.AddTransient<DisplayUpdatedCategoryViewModel>();
        services.AddTransient<DisplayAdminCategoryDetailViewModel>();
        services.AddTransient<DisplaySelectedProductFromAdminCategoryDetailViewModel>();
        services.AddTransient<ViewCategoriesForAdminViewModel>();
        services.AddTransient<AdminChooseCategoryForUpdateViewModel>();
        services.AddTransient<UpdateCategoryViewModel>();

        services.AddTransient<ProductOperationsViewModel>();
        services.AddTransient<AddProductViewModel>();
        services.AddTransient<DisplayAddedProductViewModel>();
        services.AddTransient<DisplayUpdatedProductViewModel>();
        services.AddTransient<DisplayAdminProductViewModel>();
        services.AddTransient<DisplayCustomerProductViewModel>();
        services.AddTransient<AdminChooseProductForUpdateViewModel>();
        services.AddTransient<ViewProductsForAdminViewModel>();
        services.AddTransient<UpdateProductViewModel>();
        services.AddTransient<DeleteProductViewModel>();
        services.AddTransient<RestoreProductViewModel>();

        services.AddTransient<AddAddressViewModel>();
        services.AddTransient<UpdateAddressViewModel>();
        services.AddTransient<CustomerChooseAddressForUpdateViewModel>();
        services.AddTransient<DisplayCustomerAddressForAdminViewModel>();
        services.AddTransient<DisplayAddedAddressViewModel>();
        services.AddTransient<DisplayUpdatedAddressViewModel>();
        services.AddTransient<DisplayAddressViewModel>();

        services.AddTransient<ViewCategoriesForSaleViewModel>();
        services.AddTransient<ViewProductsForSaleViewModel>();
        services.AddTransient<DisplayProductDetailForSaleViewModel>();
        services.AddTransient<CheckoutViewModel>();
        services.AddTransient<DisplayOrderDetailsViewModel>();
        services.AddTransient<DisplayCustomerOrderDetailViewModel>();
        services.AddTransient<ViewCartViewModel>();

        services.AddTransient<CustomerOperationsViewModel>();
        services.AddTransient<ViewCustomersForAdminViewModel>();
        services.AddTransient<DisplayCustomerDetailsForAdminViewModel>();
        services.AddTransient<DisplayCustomerOrderDetailForAdminViewModel>();
        services.AddTransient<DisplayCustomerProfileViewModel>();

        var serviceProvider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();

            var mainWindowViewModel = serviceProvider
                .GetRequiredService<MainWindowViewModel>();

            ShowWelcomeView();

            void ShowWelcomeView()
            {
                var welcomePageViewModel = serviceProvider.GetRequiredService<WelcomePageViewModel>();

                welcomePageViewModel.LoginRequested += ShowLoginView;
                welcomePageViewModel.RegistrationRequested += ShowRegistrationView;
                welcomePageViewModel.PasswordResetRequested += ShowPasswordResetView;

                mainWindowViewModel.CurrentView = welcomePageViewModel;
            }

            void ShowLoginView()
            {
                var loginViewModel = serviceProvider.GetRequiredService<LoginViewModel>();
                loginViewModel.LoginSuccessful += OnLoginSuccessful;
                loginViewModel.BackRequested += ShowWelcomeView;
                mainWindowViewModel.CurrentView = loginViewModel;
            }

            void ShowRegistrationView()
            {
                var registrationViewModel = serviceProvider.GetRequiredService<RegistrationViewModel>();
                registrationViewModel.RegistrationSuccessful += ShowLoginView;
                registrationViewModel.BackRequested += ShowWelcomeView;
                mainWindowViewModel.CurrentView = registrationViewModel;
            }

            void ShowPasswordResetView()
            {
                var passwordResetViewModel = serviceProvider.GetRequiredService<PasswordResetViewModel>();
                passwordResetViewModel.BackRequested += ShowWelcomeView;
                passwordResetViewModel.LoginRequested += ShowLoginView;
                mainWindowViewModel.CurrentView = passwordResetViewModel;
            }

            void OnLoginSuccessful(bool isAdmin)
            {
                var authService = serviceProvider.GetRequiredService<IAuthService>();
                var messenger = serviceProvider.GetRequiredService<IMessenger>();
                var mainUserViewModel = new MainUserViewModel(isAdmin, serviceProvider, authService, messenger);
                mainUserViewModel.LogoutRequested += OnLogoutRequested;

                mainWindowViewModel.CurrentView = mainUserViewModel;
            }

            void OnLogoutRequested()
            {
                ShowWelcomeView();
            }

            desktop.MainWindow = new MainWindow
            {
                DataContext = mainWindowViewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }

    void LoggingSetup()
    {
        var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        Directory.CreateDirectory(loggingDirectory);
        var filePath = Path.Combine(loggingDirectory, "app-.txt");
        var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.File(
            path: filePath,
            rollingInterval: RollingInterval.Day,
            outputTemplate: outputTemplate)
            .CreateLogger();
    }
}