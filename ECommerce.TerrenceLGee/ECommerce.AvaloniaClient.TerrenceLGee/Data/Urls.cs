namespace ECommerce.AvaloniaClient.TerrenceLGee.Data;

public static class Urls
{
    public static string BaseUrl => "https://localhost:7001/api/";
    public static string RegisterUrl => "auth/register";
    public static string LoginUrl => "auth/login";
    public static string PasswordResetUrl => "auth/reset";
    public static string LogoutUrl => "auth/logout";
    public static string AddAddressUrl => "addresses/add";
    public static string UpdateAddressUrl => "addresses/update/";
    public static string DeleteAddressUrl => "addresses/";
    public static string GetAddressForCustomerUrl => "addresses/";
    public static string GetCustomerAddressForAdminUrl => "addresses/admin/";
    public static string GetAllAddressesForCustomerUrl => "addresses";
    public static string GetAllAddressesForAdminUrl => "addresses/admin";
    public static string AdminAddCategoryUrl => "categories/admin/add";
    public static string AdminUpdateCategoryUrl => "categories/admin/update/";
    public static string AdminGetCategoryByIdUrl => "categories/admin/";
    public static string AdminGetCategoriesUrl => "categories/admin";
    public static string CustomerGetCategoryByIdUrl => "categories/";
    public static string CustomerGetCategoriesUrl => "categories";
    public static string CustomerGetProfileUrl => "customers/profile";
    public static string GetAllCustomersForAdminUrl => "customers/admin";
    public static string AdminAddProductUrl => "products/admin/add";
    public static string AdminUpdateProductUrl => "products/admin/update/";
    public static string AdminDeleteProductUrl => "products/admin/delete/";
    public static string AdminRestoreProductUrl => "products/admin/restore/";
    public static string CustomerGetProductByIdUrl => "products/";
    public static string CustomerGetProductsUrl => "products";
    public static string AdminGetProductByIdUrl => "products/admin/";
    public static string AdminGetProductsUrl => "products/admin";
    public static string CustomerCreateSaleUrl => "sales/checkout";
    public static string CustomerGetSaleByIdUrl => "sales/";
    public static string CustomerGetSalesUrl => "sales";
    public static string CustomerCancelSaleUrl => "sales/cancel/";
    public static string AdminGetSaleByIdUrl => "sales/admin/";
    public static string AdminGetAllSalesUrl => "sales/admin";
    public static string AdminUpdateSaleStatusUrl => "sales/admin/update/";
}