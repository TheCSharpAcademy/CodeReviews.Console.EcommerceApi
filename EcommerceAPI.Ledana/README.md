# 🛒 E-Commerce API

A backend e-commerce application built with **ASP.NET Core Web API** following **Clean Architecture** principles.
The project focuses on scalable backend development, relational database design, and RESTful API practices using modern .NET technologies.

---

# 🚀 Features

* RESTful ASP.NET Core Web API
* Clean Architecture structure
* CRUD operations for Products, Categories, and Sales
* Many-to-many relationship handling
* Entity Framework Core with SQL Server
* DTOs and AutoMapper mapping
* Authentication & Authorization
* Server-side Pagination
* Filtering & Sorting
* Dependency Injection
* Async/Await operations
* Model Validation
* Global Exception Handling
* Console Client consuming API endpoints using HttpClient
* Git & Version Control workflow

---

# 🛠 Tech Stack

## Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* LINQ
* HttpClient

## Architecture & Concepts

* Clean Architecture
* Repository & Service Patterns
* SOLID Principles
* RESTful APIs
* DTOs
* Dependency Injection
* Async Programming

## Tools

* Git
* GitHub
* Swagger/OpenAPI
* Visual Studio

---

# 📂 Project Structure

```bash
EcommerceAPI.Ledana
│
├── EcommerceAPI.API           # API layer / Controllers
├── EcommerceAPI.Application   # Business logic & services
├── EcommerceAPI.Domain        # Entities & domain models
├── EcommerceAPI.Infrastructure# Database & repository implementations
├── EcommerceAPI.ConsoleClient # Console application consuming the API
```

---

# 🔐 Authentication & Authorization

The API includes authentication and authorization mechanisms to secure endpoints and control user access.

Examples:

* Protected endpoints
* Role-based authorization
* User authentication flow

---

# 📦 Main Functionalities

## Products

* Create products
* Update products
* Delete products
* Retrieve products
* Pagination, filtering & sorting

## Categories

* Category management
* Product-category relationships

## Sales

* Sales tracking
* Product-sale relationships
* Transaction handling

---

# 🗄 Database

The application uses **SQL Server** with **Entity Framework Core** migrations.

Features include:

* Relational database design
* Many-to-many relationships
* Fluent API configurations
* Code-first migrations

---

# 🔄 API Consumption

A separate console client application consumes the API using:

* HttpClient
* JSON serialization/deserialization
* Asynchronous HTTP requests

This demonstrates a decoupled client-server architecture.

---

# ▶️ Getting Started

## Clone the Repository

```bash
git clone https://github.com/Ledana/EcommerceAPI.Ledana.git
```

## Navigate to the Project

```bash
cd EcommerceAPI.Ledana
```

## Update Database

```bash
update-database
```

## Run the API

```bash
dotnet run
```

---

# 📘 Learning Goals

This project was built to strengthen knowledge in:

* ASP.NET Core Web API development
* Clean Architecture
* EF Core & SQL Server
* Authentication & Authorization
* REST API design
* Layered application structure
* Backend best practices

---

# 📌 Future Improvements

* ASP.NET Core MVC frontend
* Docker containerization
* Azure deployment
* Integration testing
* Refresh tokens
* Logging & monitoring
* Unit & integration test coverage

---

# 👩‍💻 Author

## [Ledana Gjoka GitHub](https://github.com/Ledana)

Junior Backend .NET Developer passionate about backend development, clean code, and scalable application design.

Repository: [EcommerceAPI.Ledana](https://github.com/Ledana/EcommerceAPI.Ledana.git)
