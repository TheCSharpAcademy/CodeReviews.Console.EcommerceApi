🛍️ EcommerceAPI.Ledana
An ASP.NET Core Web API for managing e-commerce operations — including product listings, categories, orders, and user management. This project demonstrates clean architecture principles, Entity Framework Core integration, and RESTful API design for scalable backend development.

🚀 Features
Product Management: CRUD operations for products, categories, and inventory

Order Processing: Create, update, and track customer orders

User Authentication: Secure login and registration using JWT tokens

Database Integration: Built with Entity Framework Core and SQL Server

DTOs and AutoMapper: Clean data transfer between layers

Error Handling & Logging: Structured responses and middleware-based logging

Swagger Documentation: Interactive API testing and documentation

🧱 Project Structure
Folder	Description
EcommerceAPI.Ledana	Core API project containing controllers, models, and services
ECommerceUI.Ledana	Frontend/UI layer (if applicable) for interacting with the API
EcommerceAPI.Ledana.slnx	Solution file for Visual Studio setup


⚙️ Technologies Used
ASP.NET Core 8.0

Entity Framework Core

SQL Server

AutoMapper

Swagger / Swashbuckle

JWT Authentication

🧩 Getting Started
Prerequisites
Visual Studio 2022 or VS Code

.NET SDK 8.0+

SQL Server (local or remote instance)

Installation
Clone the repository:

bash
git clone https://github.com/Ledana/EcommerceAPI.Ledana.git
Navigate to the project folder:

bash
cd EcommerceAPI.Ledana
Restore dependencies:

bash
dotnet restore
Update the connection string in appsettings.json:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EcommerceDB;Trusted_Connection=True;"
}
Apply migrations:

bash
dotnet ef database update
Run the project:

bash
dotnet run
🧪 API Documentation
Once running, open Swagger UI at:
👉 https://localhost:5001/swagger

You can test endpoints for:

/api/products

/api/orders

/api/users

🤝 Contributing
Contributions are welcome!

Fork the repository

Create a new branch (feature/your-feature)

Commit your changes

Submit a pull request

📄 License
This project is licensed under the MIT License — feel free to use and modify it.
