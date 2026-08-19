using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using silvermax.ecommerceapi.Data;
using silvermax.ecommerceapi.Dtos;
using silvermax.ecommerceapi.Models;

namespace silvermax.ecommerceapi.Service;

public class EcommerceService(
    EcommerceDbContext db) : IEcommerceService
{
    public async Task<CategoryResponse?> AddCategory(AddCategoryDto dto, CancellationToken ct)
    {
        var category = await db.Categories.AnyAsync(c => c.Name == dto.Name, ct);

        if (category)
            return null;

        var newCategory = new Category
        {
            CategoryId = Guid.NewGuid(),
            Name = dto.Name
        };

        db.Categories.Add(newCategory);
        await db.SaveChangesAsync(ct);

        return new CategoryResponse(newCategory.CategoryId, newCategory.Name);
    }

    public async Task<AddProductResponseDto> AddProduct(AddProductDto dto, CancellationToken ct)
    {
        var product = await db.Products.AnyAsync(p => p.Name == dto.Name, ct);

        if (product)
            return new AddProductResponseDto(null, AddProductError.ProductAlreadyExists);

        var category = await db.Categories.FirstOrDefaultAsync(c => c.Name == dto.Category, ct);

        if (category is null)
            return new AddProductResponseDto(null, AddProductError.CategoryNotFound);


        var newProduct = new Product
        {
            ProductId = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            ImgUrl = dto.ImgUrl,
            Category = category!,
            CategoryId = category!.CategoryId
        };
        db.Products.Add(newProduct);
        category.Products.Add(newProduct);
        await db.SaveChangesAsync(ct);

        var response = new ProductResponseDto(
            newProduct.ProductId, newProduct.Name, newProduct.Description,
            newProduct.Price, newProduct.ImgUrl, category.CategoryId, category.Name);

        return new AddProductResponseDto(response, null);
    }

    public async Task<Client?> CreateClient(CreateClientDto dto, CancellationToken ct)
    {
        var client = await db.Clients.AnyAsync(c => c.Email == dto.Email, ct);

        if (client)
            return null;

        var newClient = new Client
        {
            ClientId = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.PhoneNumber
        };

        db.Clients.Add(newClient);
        await db.SaveChangesAsync(ct);

        return newClient;
    }

    public async Task<PlaceOrderResponseDto> PlaceOrder(PlaceOrderDto dto, CancellationToken ct)
    {
        var client = await db.Clients.FirstOrDefaultAsync(c => c.ClientId == dto.ClientId, ct);

        if (client is null)
            return new PlaceOrderResponseDto(null, PlaceOrderError.ClientNotFound);

        if (dto.Items.Count == 0)
            return new PlaceOrderResponseDto(null, PlaceOrderError.EmptyOrder);

        var newOrder = new Order
        {
            OrderId = Guid.NewGuid(),
            Moment = DateTime.UtcNow,
            OrderStatus = OrderStatus.WAITING_PAYMENT,
            ClientId = dto.ClientId
        };

        double total = 0;

        foreach (var line in dto.Items)
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == line.ProductId, ct);

            if (product is null)
                return new PlaceOrderResponseDto(null, PlaceOrderError.ProductNotFound);

            var existingLine = newOrder.OrderItems.FirstOrDefault(oi => oi.ProductId == line.ProductId);
            if (existingLine is not null)
            {
                existingLine.Quantity += line.Quantity;
                total += (product.Price * line.Quantity);
                continue;
            }

            var orderItem = new OrderItem
            {
                OrderItemId = Guid.NewGuid(),
                Quantity = line.Quantity,
                Price = product.Price,
                OrderId = newOrder.OrderId,
                Order = newOrder,
                ProductId = product.ProductId,
                Product = product
            };

            newOrder.OrderItems.Add(orderItem);
            total += (orderItem.Price * orderItem.Quantity);
        }

        newOrder.Total = total;
        db.Orders.Add(newOrder);
        await db.SaveChangesAsync(ct);

        var response = new OrderResponse(
            newOrder.OrderId, newOrder.Moment, newOrder.OrderStatus,
            newOrder.Total, client.ClientId, client.Name,
            newOrder.OrderItems
                .Select(oi => new OrderItemResponse(oi.ProductId, oi.Product.Name, oi.Quantity, oi.Price))
                .ToList()
        );

        return new PlaceOrderResponseDto(response, null);
    }
}
