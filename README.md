# ECommerce API

A fully-featured RESTful API built with ASP.NET Core for managing an e-commerce system. This project demonstrates modern web API development practices including clean architecture, Entity Framework Core, FluentValidation, and comprehensive API documentation.

## 📋 Project Overview

This ECommerce API is a backend system designed to handle the core operations of an online store. The API provides complete CRUD (Create, Read, Update, Delete) functionality for managing products, categories, and sales transactions. Built as part of [The C# Academy](https://thecsharpacademy.com/project/18/ecommerce-api) curriculum, it showcases best practices in .NET development.

### Key Features

- **Product Management**: Full CRUD operations for products with category associations
- **Category Management**: Organize products into hierarchical categories with soft-delete support
- **Sales Tracking**: Record and query sales transactions with detailed line items
- **Advanced Querying**: Pagination, filtering, and sorting across all resources
- **Data Validation**: Request validation using FluentValidation
- **API Documentation**: Interactive API documentation using Swagger/OpenAPI and Scalar
- **Structured Logging**: Comprehensive logging with Serilog
- **Error Handling**: Global exception handling middleware with standardized error responses
- **Database Seeding**: Automatic database initialization with sample data
- **Output Caching**: Performance optimization through response caching

## 🏗️ Architecture

The project follows a clean, layered architecture:

- **Controllers**: Handle HTTP requests and responses
- **Services**: Business logic layer
- **Repositories**: Data access layer with Entity Framework Core
- **Models**: Domain entities and DTOs (Data Transfer Objects)
- **Validators**: Input validation using FluentValidation
- **Middleware**: Cross-cutting concerns (exception handling, logging)

## 🛠️ Technology Stack

### Framework

- **.NET 10.0** - Latest .NET framework

### NuGet Packages

| Package | Version | Purpose |
| ------- | ------- | ------- |
| `Microsoft.AspNetCore.OpenApi` | 10.0.1 | OpenAPI/Swagger support |
| `Microsoft.EntityFrameworkCore` | 10.0.0 | ORM for database operations |
| `Microsoft.EntityFrameworkCore.Abstractions` | 10.0.0 | EF Core interfaces |
| `Microsoft.EntityFrameworkCore.Sqlite` | 10.0.0 | SQLite database provider |
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.0 | SQL Server database provider |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.0 | EF Core CLI tools for migrations |
| `FluentValidation.AspNetCore` | 11.3.1 | Request validation framework |
| `Swashbuckle.AspNetCore` | 10.1.0 | Swagger/OpenAPI documentation |
| `Scalar.AspNetCore` | 2.8.4 | Modern API documentation UI |
| `Serilog` | 4.0.1 | Structured logging |
| `Serilog.AspNetCore` | 8.0.1 | Serilog integration for ASP.NET Core |
| `Microsoft.CodeAnalysis.NetAnalyzers` | 9.0.0 | Code quality analyzers |

## 📦 Installation

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or use SQLite for local development)
- [Git](https://git-scm.com/downloads)
- An IDE such as [Visual Studio](https://visualstudio.microsoft.com/), [Rider](https://www.jetbrains.com/rider/), or [VS Code](https://code.visualstudio.com/)

### Steps

1. **Clone the repository**

   ```bash
   git clone https://github.com/RyanW84/EcommerceAPI.git
   cd EcommerceAPI
   ```

2. **Restore NuGet packages**

   ```bash
   dotnet restore
   ```

3. **Configure the database connection**

   Update the connection string in `appsettings.json` or `appsettings.Development.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=true;"
     }
   }
   ```

   For SQLite (development):

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=ecommerce.db"
     }
   }
   ```

4. **Apply database migrations**

   ```bash
   dotnet ef database update
   ```

   This will create the database schema and seed it with sample data.

5. **Run the application**

   ```bash
   dotnet run
   ```

   The API will start on `https://localhost:5001` (HTTPS) and `http://localhost:5000` (HTTP).

6. **Access the API documentation**
   - Swagger UI: `https://localhost:5001/swagger`
   - Scalar UI: `https://localhost:5001/scalar/v1`

## 🔌 API Endpoints

### Products

- `GET /api/v1/products` - Get all products (with pagination)
- `GET /api/v1/products/{id}` - Get product by ID
- `GET /api/v1/products/category/{categoryId}` - Get products by category
- `POST /api/v1/products` - Create a new product
- `PUT /api/v1/products/{id}` - Update a product
- `DELETE /api/v1/products/{id}` - Delete a product

### Categories

- `GET /api/v1/categories` - Get all categories (with pagination)
- `GET /api/v1/categories/{id}` - Get category by ID
- `GET /api/v1/categories/name/{name}` - Get category by name
- `POST /api/v1/categories` - Create a new category
- `PUT /api/v1/categories/{id}` - Update a category
- `DELETE /api/v1/categories/{id}` - Delete a category (soft delete)
- `POST /api/v1/categories/{id}/restore` - Restore a soft-deleted category

### Sales

- `GET /api/v1/sales` - Get all sales (with pagination)
- `GET /api/v1/sales/{id}` - Get sale by ID
- `GET /api/v1/sales/summary` - Get sales summary report
- `POST /api/v1/sales` - Create a new sale
- `PUT /api/v1/sales/{id}` - Update a sale
- `DELETE /api/v1/sales/{id}` - Delete a sale

## 🎯 RESTful Compliance

This API adheres to REST (Representational State Transfer) architectural principles:

### 1. **Resource-Based URLs**

- Resources are identified by nouns (products, categories, sales)
- Hierarchical structure: `/api/v1/products/{id}`
- No verbs in URLs (actions are defined by HTTP methods)

### 2. **HTTP Methods (Verbs)**

- `GET` - Retrieve resources (idempotent, safe)
- `POST` - Create new resources
- `PUT` - Update existing resources (idempotent)
- `DELETE` - Remove resources (idempotent)

### 3. **Status Codes**

   The API returns appropriate HTTP status codes:

- `200 OK` - Successful GET, PUT, PATCH
- `201 Created` - Successful POST with Location header
- `204 No Content` - Successful DELETE
- `400 Bad Request` - Validation errors
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server errors

### 4. **Stateless Communication**

- Each request contains all necessary information
- No server-side session state
- Authentication (if implemented) via tokens

### 5. **HATEOAS (Hypermedia)**

- Responses include `Link` headers for resource relationships
- Example: `Link: </api/v1/products/1>; rel="self"`

### 6. **Content Negotiation**

- Supports JSON format (application/json)
- Request: `Content-Type: application/json`
- Response: `Content-Type: application/json`

### 7. **Versioning**

- API versioning through URL path: `/api/v1/`
- Allows for backwards compatibility

### 8. **Pagination & Filtering**

- Query parameters for filtering: `?category=electronics`
- Pagination: `?page=1&pageSize=10`
- Sorting: `?sortBy=name&sortOrder=desc`

### 9. **Standardized Response Format**

   All responses follow a consistent structure:

   ```json
   {
     "data": { },
     "responseCode": 200,
     "errorMessage": null,
     "requestFailed": false
   }
   ```

### 10. **Idempotency**

- GET, PUT, DELETE operations are idempotent
- Multiple identical requests produce the same result

## 🧪 Testing

The project includes comprehensive test coverage:

- **Unit Tests** (`tests/ECommerceApp.UnitTests/`)
- **Integration Tests** (`tests/ECommerceApp.IntegrationTests/`)
- **Architecture Tests** (`tests/ArchitectureTests/`)

Run all tests:

```bash
dotnet test
```

## 🔐 Configuration

### Environment Settings

The application supports multiple environments:

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production settings

### Key Configuration Options

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*",
  "Scalar": {
    "Title": "ECommerce API Documentation",
    "Version": "v1"
  }
}
```

## 📊 Database

The application uses Entity Framework Core with support for:

- **SQL Server** (production)
- **SQLite** (development)

### Migrations

Create a new migration:

```bash
dotnet ef migrations add MigrationName
```

Update database:

```bash
dotnet ef database update
```

Rollback migration:

```bash
dotnet ef database update PreviousMigrationName
```

## 🐳 Docker Support

Build Docker image:

```bash
docker build -t ecommerce-api .
```

Run with Docker Compose:

```bash
docker-compose up
```

## 📝 Console Client

The project includes a console client application (`ConsoleClient/`) for testing and interacting with the API programmatically.

Run the console client:

```bash
cd ConsoleClient
dotnet run
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the terms specified in the LICENSE file.

## 🙏 Acknowledgments

- [The C# Academy](https://thecsharpacademy.com/) for the project inspiration and requirements
- Microsoft for the excellent .NET documentation
- The open-source community for the amazing packages used in this project

## 📧 Contact

Ryan W84 - [@RyanW84](https://github.com/RyanW84)

Project Link: [https://github.com/RyanW84/EcommerceAPI](https://github.com/RyanW84/EcommerceAPI)

---

**Note**: This is a portfolio/learning project. For production use, additional security measures (authentication, authorization, rate limiting, etc.) should be implemented.
