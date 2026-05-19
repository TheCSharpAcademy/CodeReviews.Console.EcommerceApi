namespace ECommerce.UI.Enums;

//------- Administrator Ui Enums -------
internal enum AdminMainMenu
{
    ManageProducts,
    ManageProductTags,
    ManageSales,
    EnterTestingEnvironment,
    ExitApplication
}

internal enum ManageProductsMenuOption
{
    ReviewProducts,
    SearchProducts,
    CreateNewProduct,
    DeleteProduct,
    Back
}

internal enum PaginationController
{
    LastPage,
    NextPage,
    Back
}

internal enum PaginationControllerWithSelection
{
    LastPage,
    NextPage,
    SelectProduct,
    Back
}

internal enum SearchController
{
    SearchByTerm,
    FilterByTags,
    FilterByGenre,
}

internal enum SearchTagsController
{
    
    SearchForSpecificTag,
    BrowseAllTags,
    Back
}

internal enum TagAdditionMethodForItem
{
    SearchForAnExistingTag,
    CreateNewTag,
    CreateWithoutTags
}

internal enum ManageProductTagsMenu
{
    ReviewTags,
    SearchTags,
    CreateNewTag,
    DeleteTag,
    Back
}

internal enum ManageSalesMenu
{
    ReviewSales,
    CreateNewSale,
    DeleteSale,
    Back
}

//------- Testing Ui Enums -------
internal enum TestingMenuOption
{
    BrowseProducts,
    SearchProducts,
    Checkout,
    ExitTestingEnvironment
}

internal enum PaginationControllerWithAddToCart
{
    LastPage,
    NextPage,
    AddToCart,
    Back
}

internal enum CheckoutMenu
{
    CheckoutItems,
    RemoveItem,
    ClearAllItems,
    Back
}