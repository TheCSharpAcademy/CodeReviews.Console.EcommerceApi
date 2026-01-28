# E-commerce Web API

A robust RESTful API built with *.NET 10* and **Entity Framework Core**, designed to manage products, categories, and sales transactions for an e-commerce platform.

## Project Overview

This backend application demonstrates a production-ready architecture using the Controller-Service pattern and Dependency Injection. It provides a comprehensive set of endpoints for managing inventory and processing sales with transactional integrity.

## Key Features

* **Architecture**: Built using the Controller-Service pattern with Dependency Injection for maintainability and testability.
* **Database Integration**: Utilizes SQLite with Entity Framework Core for data persistence.
* **Complex Relationships**: Implements Many-to-Many relationships between Products and Sales.
* **Transactional Integrity**: Sales are created atomically. Product prices are snapshot at the time of purchase to ensure historical accuracy, protecting records from future price changes.
* **Advanced Querying**: All data retrieval endpoints support Server-Side Pagination, Filtering, and Sorting to handle large datasets efficiently.
* **Data Safety**: Implements "Soft Deletes" to prevent accidental data loss.
* **Automated Data Seeding**: The application automatically populates the database with realistic test data upon startup.

## Technology Stack

* **Framework**: .NET  10 Web API
* **Language**: C#
* **ORM**: Entity Framework Core
* **Database**: SQLite
* **Testing**: xUnit

## API Endpoints

The API provides a wide range of endpoints to manage resources. All list endpoints support dynamic query parameters for flexible data retrieval.

### Products

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/products` | Retrieve a paginated list of products. |
| `GET` | `/api/products/{id}` | Retrieve details of a specific product. |

**Supported Query Parameters:**
* `searchTerm`: Filter by product name (Partial match).
* `minPrice`: Filter products above a specific price.
* `maxPrice`: Filter products below a specific price.
* `categoryId`: Filter products belonging to a specific category.
* `sortBy`: Sort results by `price_asc`, `price_desc`, `name`, or `name_desc`.
* `pageIndex`: Specify the page number (Default: 1).
* `pageSize`: Specify the number of items per page (Default: 10).

---

### Sales (Orders)

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/sales` | Retrieve the full history of sales transactions. |
| `GET` | `/api/sales/{id}` | Retrieve a specific sale and its associated line items. |
| `POST` | `/api/sales` | Create a new sale transaction. |

**Supported Query Parameters:**
* `fromDate`: Filter sales starting from a specific date (Format: YYYY-MM-DD).
* `toDate`: Filter sales up to a specific date.
* `minAmount`: Filter orders with a total value above a specified amount.
* `productId`: Retrieve all sales containing a specific product ID.
* `sortBy`: Sort results by `date_desc`, `date_asc`, `amount_desc`, or `amount_asc`.
* `pageIndex` / `pageSize`: Pagination controls.

---

### Categories

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/categories` | List all available product categories. |
| `GET` | `/api/categories/{id}` | Retrieve details for a specific category. |

**Supported Query Parameters:**
* `searchTerm`: Search for categories by name.
* `sortBy`: Sort results by `name` or `name_desc`.
* `pageIndex` / `pageSize`: Pagination controls.

