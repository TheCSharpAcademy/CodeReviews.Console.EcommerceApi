# E-commerce API

A RESTful API built with ASP.NET Core for managing an e-commerce system with products, categories, and sales tracking.

## Features

- **Product Management**: CRUD operations for products with inventory tracking
- **Category Management**: Organize products into categories
- **Sales Processing**: Create and track sales with automatic inventory deduction
- **Authentication & Authorization:** Secure, stateless user authentication utilizing ASP.NET Core Identity and JSON Web Tokens (JWT).
  Implements role-based access control to lock down administrative endpoints while safely exposing public browsing routes.
- **Soft Delete**: Entities are soft-deleted, allowing data recovery
- **Pagination**: Efficient data retrieval with pagination support
- **Result Pattern**: Consistent error handling and response formatting
- **Global Exception Handling**: Middleware for centralized error handling across all requests
- **Input Validation**: Request validation using FluentValidation with descriptive error messages
- **API Versioning**: URL segment versioning via Asp.Versioning (current version: v1)

## Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
EcommerceApi/
├── Ecommerce.Core/          # Domain layer (Models, DTOs, Interfaces)
│   ├── Models/              # Domain entities
│   ├── DTOs/                # Data Transfer Objects
│   ├── Interfaces/          # Service and repository contracts
│   ├── Utilities/           # Shared utilities (Result, Pagination)
|   └── Validators/          # FluentValidation validators
├── Ecommerce.Data/          # Data access layer
│   ├── Repositories/        # Repository implementations
│   ├── Migrations/          # EF Core migrations
│   └── AppDbContext.cs      # Database context
├── Ecommerce.Services/      # Business logic layer
│   ├── CategoryService.cs
│   ├── ProductService.cs
│   └── SaleService.cs
└── Ecommerce.Web/           # Presentation layer (API)
    ├── Controllers/         # API controllers
    ├── Extensions/          # Global Exception handler
    └── Program.cs           # Application entry point
```

## Technologies

- **.NET 10.0**
- **ASP.NET Core Web API**
- **Entity Framework Core** with SQL Server
- **Repository Pattern** with Unit of Work
- **Dependency Injection**
- **OpenAPI/Postman** for API documentation
- **FluentValidation** for input validation
- **Asp.Versioning** for API versioning
- **Json Web Tokens** for token-based Authentication and Authorization

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full version)
- Visual Studio 2022 / JetBrains Rider / VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd EcommerceApi
   ```

2. **Configure the database connection**

   Update the connection string in `Ecommerce.Web/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EcommerceDb;Trusted_Connection=true;"
     }
   }
   ```

3. **Apply database migrations**
   ```bash
   cd Ecommerce.Web
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run --project Ecommerce.Web
   ```

5. **Access the API**
   - API: `http://localhost:5248`
   - OpenAPI UI: Navigate to `/openapi/v1.json` (in development mode)

## API Endpoints

### Categories

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/category` | Get all categories |
| GET | `/api/v1/category/{id}` | Get category by ID |
| POST | `/api/v1/category` | Create a new category |
| PUT | `/api/v1/category/{id}` | Update a category |
| DELETE | `/api/v1/category/{id}` | Delete a category (soft delete) |

### Products

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/product?pageNumber=1&pageSize=10` | Get all products (paginated) |
| GET | `/api/v1/product/{id}` | Get product by ID |
| POST | `/api/v1/product` | Create a new product |
| PUT | `/api/v1/product/{id}` | Update a product |
| DELETE | `/api/v1/product/{id}` | Delete a product (soft delete) |

### Sales

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/sale?pageNumber=1&pageSize=10` | Get all sales (paginated) |
| GET | `/api/v1/sale/{id}` | Get sale by ID with items |
| POST | `/api/v1/sale` | Create a new sale |

## API Usage Examples

### Create a Category
 
```http
POST /api/v1/category
Content-Type: application/json
 
{
  "name": "Electronics",
  "description": "Electronic devices and gadgets"
}
```
 
### Create a Product
 
```http
POST /api/v1/product
Content-Type: application/json
 
{
  "name": "Laptop",
  "description": "High-performance laptop",
  "price": 999.99,
  "quantity": 50,
  "imageUrl": "https://example.com/laptop.jpg",
  "categoryId": 1
}
```
 
### Create a Sale
 
```http
POST /api/v1/sale
Content-Type: application/json
 
{
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 2,
      "quantity": 1
    }
  ]
}
```

**Note**: Creating a sale automatically:
- Validates product availability
- Deducts inventory quantities
- Captures prices at time of sale
- Calculates total price

### Pagination

Products and Sales support pagination:

```http
GET /api/v1/product?pageNumber=2&pageSize=20
```

## Database Schema

### Core Entities

**Category**
- `Id` (int, PK)
- `Name` (string, required)
- `Description` (string, nullable)
- `IsDeleted` (bool)
- `DeletedAt` (DateTime?)

**Product**
- `Id` (int, PK)
- `Name` (string, required)
- `Price` (decimal(18,2))
- `Quantity` (int)
- `Description` (string, nullable)
- `ImageUrl` (string, nullable)
- `CategoryId` (int, FK)
- `IsDeleted` (bool)
- `DeletedAt` (DateTime?)

**Sale**
- `Id` (int, PK)
- `CreationDate` (DateTime)
- `TotalPrice` (decimal(18,2))
- `IsDeleted` (bool)
- `DeletedAt` (DateTime?)

**SaleItem** (Many-to-Many)
- `SaleId` (int, PK, FK)
- `ProductId` (int, PK, FK)
- `Quantity` (int)
- `UnitPriceAtTimeOfSale` (decimal(18,2))

## Design Patterns

- **Repository Pattern**: Abstracts data access logic
- **Unit of Work**: Manages transactions across repositories
- **Result Pattern**: Standardized success/failure responses
- **Dependency Injection**: Loose coupling between layers
- **Soft Delete**: Data preservation with `ISoftDeletable` interface

## Development

### Project Structure

- **Ecommerce.Core**: Contains business entities, DTOs, and interfaces (no dependencies)
- **Ecommerce.Data**: Implements data access using EF Core
- **Ecommerce.Services**: Implements business logic
- **Ecommerce.Web**: ASP.NET Core Web API project

### Adding Migrations

```bash
dotnet ef migrations add MigrationName --project Ecommerce.Data --startup-project Ecommerce.Web
```

### Applying Migrations

```bash
dotnet ef database update --project Ecommerce.Data --startup-project Ecommerce.Web
```

## Error Handling

The API uses a custom `Result<T>` pattern for consistent error responses:

**Success Response:**
```json
{
  "data": { ... },
  "isSuccess": true,
  "message": null
}
```

**Error Response:**
```json
{
  "data": null,
  "isSuccess": false,
  "message": "Error description"
}
```

## Validation

Input validation is handled by FluentValidation and covers:

- Required fields (e.g. product name, category name)
- Positive quantity values
- Price must be greater than zero
- Category existence when creating/updating products
- Product existence and stock availability when creating sales

## API Versioning
 
This API uses **URL segment versioning** via the `Asp.Versioning` package. The version is embedded directly in the route:
 
```
/api/v{version}/{resource}
```
 
**Current version:** `v1`
 
All endpoints are prefixed with `/api/v1/`. When new versions are introduced, older versions remain accessible at their original paths to avoid breaking changes.

## Future Enhancements

- [ ] Implement logging
- [ ] Add unit and integration tests
- [ ] Implement caching for frequently accessed data
- [ ] Implement rate limiting
- [ ] Add CORS configuration
- [ ] Add filtering and sorting capabilities
- [ ] Implement search functionality

## Contributing

This is a study project from [The C# Academy](https://thecsharpacademy.com/). Feel free to fork and experiment!
