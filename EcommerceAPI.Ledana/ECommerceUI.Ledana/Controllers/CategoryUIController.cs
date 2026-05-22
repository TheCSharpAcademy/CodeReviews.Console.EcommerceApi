using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Models;
using ECommerceUI.Ledana.Clients;
using ECommerceUI.Ledana.Services;
using ECommerceUI.Ledana.UI;

namespace ECommerceUI.Ledana.Controllers
{
    internal class CategoryUIController
    {
        static CategoryApiClient categoryApiClient = new();
        internal static async Task AddNewCategory()
        {
            string categoryName = CategoryUIService.GetCategoryName("Please put the name of the new category");
            CategoryDto categoryDto = new()
            {
                Name = categoryName
            };
            Console.WriteLine(await categoryApiClient.CreateCategory(categoryDto));
        }

        internal static async Task DeleteCategory()
        {
            await ViewCategories();
            int categoryId = await CategoryUIService.GetCategoryId("Please put the id of the category you want to delete");
            if (categoryId == 0) return;

            Console.WriteLine(await categoryApiClient.DeleteCategory(categoryId));
        }

        internal static async Task UpdateCategory()
        {
            await ViewCategories();
            int categoryId = await CategoryUIService.GetCategoryId("Please put the id of the category you want to update");
            if (categoryId == 0) return;

            string categoryName = CategoryUIService.GetCategoryName("Please put the name of the new category");
            CategoryDto categoryDto = new()
            {
                Name = categoryName
            };
            Console.WriteLine(await categoryApiClient.UpdateCategory(categoryId, categoryDto));
        }

        internal static async Task ViewCategories()
        {
            var categories = await categoryApiClient.GetCategories();
            if (categories is null || categories.Count == 0) Console.WriteLine("No categories found!");
            else
                TableVisualisation.ShowCategories(categories);
        }

        internal static async Task ViewCategoryById()
        {
            await ViewCategories();
            int categoryId = await CategoryUIService.GetCategoryId("Please put the id of the category you want to view");
            if (categoryId == 0) return;

            var category = await categoryApiClient.GetCategoryById(categoryId);
            if (category is null)
            {
                Console.WriteLine("Category not found");
                return;
            }
            TableVisualisation.ShowCategory(category);
        }
    }
}
