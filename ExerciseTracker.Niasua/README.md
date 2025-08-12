# Exercise Tracker Console App

A simple console-based application built with **.NET 8**, designed to track physical 
exercises using a clean architecture. The project implements the **Repository Pattern**, 
**Dependency Injection**, and uses **Entity Framework Core** for data access. 
It also uses **Spectre.Console** to enhance the user interface.

---

## Architecture Overview

This project follows a layered architecture:

- **UI**: Console interface using `Spectre.Console`
- **Controller**: Orchestrates user interaction with business logic
- **Service**: Handles business rules and validations
- **Repository**: Encapsulates data access logic
- **Data**: EF Core DbContext
- **Models**: Entity definitions
- **Validators**: Input and business rule validation

---

## ✅ Features

- Track exercises (e.g., weights, cardio)
- Add / View / Edit / Delete exercises
- Beautiful CLI with [Spectre.Console](https://spectreconsole.net/)
- Validations for dates and durations
- Clean separation between layers (UI, logic, data)
- **EF Core** in master branch
- **Raw SQL (Dapper)** for the separate branch

---

## 🛠 Technologies

- [.NET 8](https://dotnet.microsoft.com/)
- **Entity Framework Core** (default branch)
- **Dapper** (in `dapper` branch)
- **Spectre.Console**
- **SQL Server**
- **Dependency Injection**
