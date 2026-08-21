# EcommercAPI

An ASP.NET Core Web API for a small e-commerce backend: categories, products, clients, and orders, backed by EF Core and SQL Server.

## Tech stack

- .NET 10, ASP.NET Core Web API
- Entity Framework Core (SQL Server provider)
- Swashbuckle (Swagger/OpenAPI)
- Bogus, for generating seed data

## Setup

1. Clone the repo.
2. Set your own connection string in `appsettings.json` (or `appsettings.Development.json`) under `ConnectionStrings:DefaultConnection`. The default assumes a local SQL Server instance with Windows auth:
   ```
   "DefaultConnection": "Server=localhost;Database=Ecommerce;Trusted_Connection=True;TrustServerCertificate=True;"
   ```
3. Apply migrations:
   ```
   dotnet ef database update
   ```
4. Run the API:
   ```
   dotnet run
   ```
   In Development, the database seeds itself automatically on first run (10 categories, 50 clients, 60 products, and a batch of orders built from those), as long as the `Products` table is empty. If you've already inserted data manually, the seeder skips itself. Drop and recreate the database if you want a clean seeded run.

## Data model

- **Category** → **Product**: one-to-many. A product belongs to one category.
- **Client** → **Order**: one-to-many. A client can place many orders.
- **Order** ↔ **Product**: many-to-many, through `OrderItem`. `OrderItem` also carries `Quantity` and `Price`, where price is a snapshot of the product's price *at the time of purchase*, not a live reference to `Product.Price`. That's deliberate. If a product's price changes later, past orders still reflect what was actually paid, which is also why there's no endpoint to edit a product's price.

## Endpoints

| Method | Route | Description |
|---|---|---|
| POST | `/api/Ecommerce/add-category` | Create a category |
| POST | `/api/Ecommerce/add-product` | Create a product under an existing category |
| POST | `/api/Ecommerce/create-client` | Create a client |
| POST | `/api/Ecommerce/place-order` | Place an order for a client (one or more products, with quantities) |
| GET | `/api/Ecommerce/orders` | List orders, paginated |
| GET | `/api/Ecommerce/products` | List products, paginated |

`GET /orders` and `GET /products` both accept `pageNumber` and `pageSize` query parameters.

## Postman collection

A Postman collection covering every endpoint above, with example request bodies, is included at `postman-collection.json`. Import it into Postman and set the `baseUrl` variable to wherever the API is running (`https://localhost:7023` by default).
