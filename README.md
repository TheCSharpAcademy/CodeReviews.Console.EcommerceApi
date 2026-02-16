# Ecommerce API

A RESTful API built with **ASP.NET Core** and **Entity Framework Core**, designed to simulate a retail environment. It uses use good development practices such as Result Pattern, Service Layer, Data Transfer Objects (DTOs), Soft Deletes and historical data integrity.

## Features

- **Product, Category and Sales Management**: Full CRUD capabilities with relationship validation.
- **Historical Price Integrity**: Implements "snapshotting" logic to ensure past sales records remain accurate even if current product prices change.
- **Soft Deletes**: Implements non-destructive deletion for Products, and Sales to preserve data history, **Categories** can be deleted only there are no Products using them.
- **Pagination**: Endpoint pagination using performant `Skip`/`Take` logic and configurable page size via query parameters that prevent abuse or DDoS.
- **Result Pattern**: Uses a functional `Result<T>` wrapper to handle service-layer errors gracefully without relying on expensive Exception throwing for control flow.

## Tech Stack

- **Framework**: .NET 10 (ASP.NET Core Web API)
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **Documentation**: OpenAPI
- **Architecture**: N-Layer (Controller -> Service -> Data Access)

## Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/diegopetrola/CodeReviews.Console.EcommerceApi
   cd CodeReviews.Console.EcommerceApi
   ```

2. **Install libraries**: `dotnet restore`
3. **Update the Database**: `dotnet ef database update`

4. **Run the project** `dotnet run`

5. **Test on Postman**: import `Ecommerce.postman_collection.json` on Postman and run collection.
