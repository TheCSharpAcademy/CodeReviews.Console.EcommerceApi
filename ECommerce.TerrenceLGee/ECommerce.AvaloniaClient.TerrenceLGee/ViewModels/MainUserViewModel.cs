using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Enums;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.Enums.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class MainUserViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IAuthService _authService;
    public ObservableCollection<MenuItemViewModel> MenuItems { get; } = [];

    [ObservableProperty]
    private ObservableObject? _currentSubView;

    [ObservableProperty]
    private ObservableObject? _previousSubView;

    [ObservableProperty]
    private MenuItemViewModel? _selectedMenuItem;

    public event Action? LogoutRequested;

    public MainUserViewModel(bool IsAdmin, IServiceProvider serviceProvider, IAuthService authService, IMessenger messenger)
    {
        _serviceProvider = serviceProvider;
        _authService = authService;
        _messenger = messenger;

        if (IsAdmin)
        {
            var adminMenuItems = Enum.GetValues<AdminMenu>()
                .Select(e => new MenuItemViewModel(e.GetDisplayName(), e));

            foreach (var item in adminMenuItems)
            {
                MenuItems.Add(item);
            }
        }
        else
        {
            var customerMenuItems = Enum.GetValues<CustomerMenu>()
                .Select(e => new MenuItemViewModel(e.GetDisplayName(), e));

            foreach (var item in customerMenuItems)
            {
                MenuItems.Add(item);
            }
        }

        CurrentSubView = _serviceProvider.GetRequiredService<HomeScreenViewModel>();

        MessageRegistration();
    }

    partial void OnSelectedMenuItemChanged(MenuItemViewModel? value)
    {
        if (value is null) return;

        switch (value.Value)
        {
            case AdminMenu.Categories:
                CurrentSubView = _serviceProvider.GetRequiredService<CategoryOperationsViewModel>();
                break;
            case AdminMenu.Products:
                CurrentSubView = _serviceProvider.GetRequiredService<ProductOperationsViewModel>();
                break;
            case AdminMenu.Customers:
                CurrentSubView = _serviceProvider.GetRequiredService<CustomerOperationsViewModel>();
                break;
            case CustomerMenu.ViewProfile:
                _messenger.Send(new DisplayCustomerProfileMessage());
                break;
            case CustomerMenu.AddSale:
                CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForSaleViewModel>();
                break;
            case AdminMenu.Logout:
            case CustomerMenu.Logout:
                _authService.LogoutUserAsync();
                _messenger.UnregisterAll(this);
                LogoutRequested?.Invoke();
                break;
        }
    }

    private void MessageRegistration()
    {
        CategoryMessageRegistration();
        ProductMessageRegistration();
        AddressMessageRegistration();
        SaleMessageRegistration();
        CustomerMessageRegistration();
        OtherMessageRegistration();
    }

    private void CategoryMessageRegistration()
    {
        _messenger.Register<AddCategoryMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = _serviceProvider.GetRequiredService<AddCategoryViewModel>();
        });

        _messenger.Register<UpdateCategoryMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseCategoryForUpdateViewModel>();
        });

        _messenger.Register<NavigateBackToViewCategoriesForUpdateCategoryMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseCategoryForUpdateViewModel>();
        });

        _messenger.Register<CategoryAddedMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
            var detailVM = new DisplayAddedCategoryViewModel(m.Data, _messenger);
            CurrentSubView = detailVM;
        });

        _messenger.Register<NavigateBackToAddCategoryMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AddCategoryViewModel>();
        });

        _messenger.Register<CategoryUpdatedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayUpdatedCategoryViewModel(m.Data, _messenger);
        });

        _messenger.Register<CategorySelectedForUpdateMessage>(this, (r, m) =>
        {
            var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
            CurrentSubView = new UpdateCategoryViewModel(categoryService, m.Data, _messenger);
        });

        _messenger.Register<CategorySelectedForAdminMessage>(this, async (r, m) =>
        {
            var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            var detailVM = new DisplayAdminCategoryDetailViewModel(categoryService, productService, m.CategoryId, _messenger);
            await detailVM.GetCategoryAsync();
            CurrentSubView = detailVM;
        });


        _messenger.Register<NavigateBackToAllAdminCategoriesMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForAdminViewModel>();
        });

        _messenger.Register<NavigateBackToAdminCategoryDetailView>(this, async (r, m) =>
        {
            var categoryService = _serviceProvider.GetRequiredService<ICategoryService>();
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            var detailVM = new DisplayAdminCategoryDetailViewModel(categoryService, productService, m.CategoryId, _messenger);
            await detailVM.GetCategoryAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<NavigateBackToCategoryPageMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<CategoryOperationsViewModel>();
        });
    }

    private void ProductMessageRegistration()
    {
        _messenger.Register<AddProductMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = _serviceProvider.GetRequiredService<AddProductViewModel>();
        });

        _messenger.Register<UpdateProductMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseProductForUpdateViewModel>();
        });

        _messenger.Register<RestoreProductMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = _serviceProvider.GetRequiredService<RestoreProductViewModel>();
        });

        _messenger.Register<DeleteProductMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = _serviceProvider.GetRequiredService<DeleteProductViewModel>();
        });

        _messenger.Register<ProductAddedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayAddedProductViewModel(m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToAddProductMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AddProductViewModel>();
        });

        _messenger.Register<ProductSelectedForUpdateMessage>(this, (r, m) =>
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            CurrentSubView = new UpdateProductViewModel(productService, m.Data, _messenger);
        });

        _messenger.Register<ProductUpdatedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayUpdatedProductViewModel(m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToUpdateProductMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseProductForUpdateViewModel>();
        });

        _messenger.Register<NavigateBackToAllAdminProductsMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewProductsForAdminViewModel>();
        });

        _messenger.Register<ProductSelectedForAdminMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            var detailVM = new DisplayAdminProductViewModel(productService, m.ProductId, _messenger);
            await detailVM.GetProductAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<NavigateBackToProductPageMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ProductOperationsViewModel>();
        });

        _messenger.Register<NavigateBackToAllAdminProductsFromUpdateView>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AdminChooseProductForUpdateViewModel>();
        });

        _messenger.Register<CategoryProductSelectedForAdminMessage>(this, async (r, m) =>
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            var detailVM = new DisplaySelectedProductFromAdminCategoryDetailViewModel(productService, m.ProductId, m.CategoryId, _messenger);
            await detailVM.GetProductAsync();
            CurrentSubView = detailVM;
        });
    }

    private void AddressMessageRegistration()
    {
        _messenger.Register<CustomerAddresseSelectedForAdminMessage>(this, async (r, m) =>
        {
            var addressService = _serviceProvider.GetRequiredService<IAddressService>();
            var detailVM = new DisplayCustomerAddressForAdminViewModel(addressService, m.AddressId, m.CustomerId, _messenger);
            await detailVM.GetAddressAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<AddressAddedMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = new DisplayAddedAddressViewModel(m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToAddAddressMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AddAddressViewModel>();
        });

        _messenger.Register<AddressSelectedForUpdateMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var addressService = _serviceProvider.GetRequiredService<IAddressService>();
            CurrentSubView = new UpdateAddressViewModel(addressService, m.Data, _messenger);
        });

        _messenger.Register<NavigateBackToUpdateAddressMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<CustomerChooseAddressForUpdateViewModel>();
        });

        _messenger.Register<AddressUpdatedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayUpdatedAddressViewModel(m.Data, _messenger);
        });

        _messenger.Register<AddressSelectedForDetailMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var addressService = _serviceProvider.GetRequiredService<IAddressService>();
            var detailVM =  new DisplayAddressViewModel(addressService, m.AddressId, _messenger);
            await detailVM.GetAddressAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<AddAddressMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<AddAddressViewModel>();
        });
    }

    private void SaleMessageRegistration()
    {
        _messenger.Register<CategorySelectedForSaleMessage>(this, (r, m) =>
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            CurrentSubView = new ViewProductsForSaleViewModel(productService, m.CategoryId, m.ShoppingCart, _messenger);
        });

        _messenger.Register<NavigateBackToAllCategoriesForSale>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForSaleViewModel>();
        });

        _messenger.Register<NavigateBackToProductsFromSelectedProductMessage>(this, (r, m) =>
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            CurrentSubView = new ViewProductsForSaleViewModel(productService, m.CategoryId, m.ShoppingCart, _messenger);
        });

        _messenger.Register<ProductSelectedForSaleMessage>(this, (r, m) =>
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            CurrentSubView = new DisplayProductDetailForSaleViewModel(productService, m.ShoppingCart, m.Data, m.CategoryId, _messenger);
        });

        _messenger.Register<CheckoutMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var saleService = _serviceProvider.GetRequiredService<ISaleService>();
            CurrentSubView = new CheckoutViewModel(saleService, m.ShoppingCart, _messenger);
        });

        _messenger.Register<NavigateBackToAllCategoriesOrderCanceledMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForSaleViewModel>();
        });

        _messenger.Register<ShopAgainMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<ViewCategoriesForSaleViewModel>();
        });

        _messenger.Register<OrderCompletedMessage>(this, (r, m) =>
        {
            CurrentSubView = new DisplayOrderDetailsViewModel(m.Data, _messenger);
        });

        _messenger.Register<SaleSelectedForCustomerDetailMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var saleService = _serviceProvider.GetRequiredService<ISaleService>();
            var detailVM = new DisplayCustomerOrderDetailViewModel(saleService, m.SaleId, _messenger);
            await detailVM.GetSaleAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<ViewCartMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = new ViewCartViewModel(m.ShoppingCart, _messenger);
        });

        _messenger.Register<NavigateBackFromViewCart>(this, (r, m) =>
        {
            CurrentSubView = PreviousSubView;
        });
    }

    private void CustomerMessageRegistration()
    {
        _messenger.Register<DisplayCustomerProfileMessage>(this, async (r, m) =>
        {
            var customerService = _serviceProvider.GetRequiredService<ICustomerService>();
            var addressService = _serviceProvider.GetRequiredService<IAddressService>();
            var saleService = _serviceProvider.GetRequiredService<ISaleService>();
            var profileVM = new DisplayCustomerProfileViewModel(customerService, addressService, saleService, _messenger);
            await profileVM.GetProfileAsync();
            CurrentSubView = profileVM;
        });

        _messenger.Register<NavigateBackToCustomerPageMessage>(this, (r, m) =>
        {
            CurrentSubView = _serviceProvider.GetRequiredService<CustomerOperationsViewModel>();
        });

        _messenger.Register<ViewCustomersForAdminMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            CurrentSubView = _serviceProvider.GetRequiredService<ViewCustomersForAdminViewModel>();
        });

        _messenger.Register<DisplayCustomerDetailsForAdminMessage>(this, (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var addressService = _serviceProvider.GetRequiredService<IAddressService>();
            var saleService = _serviceProvider.GetRequiredService<ISaleService>();
            CurrentSubView = new DisplayCustomerDetailsForAdminViewModel(m.Data, addressService, saleService, _messenger);
        });

        _messenger.Register<AdminSelectedCustomerOrderForDetailMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var saleService = _serviceProvider.GetRequiredService<ISaleService>();
            var detailVM = new DisplayCustomerOrderDetailForAdminViewModel(saleService, m.SaleId, m.Data, _messenger);
            await detailVM.GetSaleAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<ViewCustomerSaleProductDetailForAdminMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            var detailVM = new DisplayAdminProductViewModel(productService, m.ProductId, _messenger);
            await detailVM.GetProductAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<NavigateBackToCustomerDetailsMessage>(this, async (r, m) =>
        {
            var addressService = _serviceProvider.GetRequiredService<IAddressService>();
            var saleService = _serviceProvider.GetRequiredService<ISaleService>();
            CurrentSubView = new DisplayCustomerDetailsForAdminViewModel(m.Data, addressService, saleService, _messenger);
        });

        _messenger.Register<DisplayCustomerAddressDetailForAdminMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var addressService = _serviceProvider.GetRequiredService<IAddressService>();
            var detailVM = new DisplayCustomerAddressForAdminViewModel(addressService, m.AddressId, m.customerId, _messenger);
            await detailVM.GetAddressAsync();
            CurrentSubView = detailVM;
        });

        _messenger.Register<SaleProductSelectedForCustomerDetailMessage>(this, async (r, m) =>
        {
            PreviousSubView = CurrentSubView;
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            var detailVM = new DisplayCustomerProductViewModel(productService, m.ProductId, _messenger);
            await detailVM.GetProductAsync();
            CurrentSubView = detailVM;
        });
    }

    private void OtherMessageRegistration()
    {
        _messenger.Register<NavigateBackToPreviousPageMessage>(this, (r, m) =>
        {
            CurrentSubView = PreviousSubView;
        });
    }
}
