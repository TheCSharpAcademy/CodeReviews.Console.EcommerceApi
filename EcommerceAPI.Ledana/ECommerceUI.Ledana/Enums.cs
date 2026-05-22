namespace ECommerceUI.Ledana
{
    internal class Enums
    {
        public enum MainMenuOptions
        {
            ProductsMenu,
            CategoriesMenu,
            SalesMenu,
            Quit
        }
        public enum ProductsMenuOptions
        {
            ViewProductsMenu,
            AddNewProduct,
            UpdateProduct,
            DeleteProduct,
            GoBack
        }
        public enum ViewProductsMenuOptions
        {
            ViewAllProductsOrderedById,
            ViewAllProductsOrderedByPrice,
            ViewAllProductsOrderedByStock,
            ViewProductById,
            ViewProductsByName,
            ViewProductsCheaperThenPrice,
            ViewProductsLowerThenStock,
            GoBack
        }
        public enum CategoriesMenuOptions
        {
            ViewCategories,
            ViewCategoryById,
            AddNewCategory,
            UpdateCategory,
            DeleteCategory,
            GoBack
        }
        public enum SalesMenuOptions
        {
            ViewSalesMenu,
            AddNewSale,
            GoBack
        }
        public enum ViewSalesMenuOptions
        {
            ViewSalesOrderedByDate,
            ViewSalesOrderedByTotalPrice,
            ViewSaleWithId,
            ViewSalesWithProductName,
            ViewSalesWithCategoryName,
            ViewSalesWithTotalPrice,
            ViewSalesCheaperThenPrice,
            GoBack
        }
    }
}
