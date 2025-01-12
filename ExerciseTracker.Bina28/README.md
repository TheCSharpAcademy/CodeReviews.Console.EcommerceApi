
# Exercise Management System

This project allows you to manage exercise records. It provides functionality to add, remove, update, and view exercise data, all while interacting with a SQL Server database.

## Features

- Add new exercise records.
- Update existing exercise records.
- Remove exercise records.
- View all exercises or a specific exercise by ID.
- Provides a simple console UI for managing exercises.

## Prerequisites

Before running the application, ensure you have the following:

- **.NET 5.0 or higher** (for building and running the application)
- **SQL Server** (or another compatible database)
- **Connection String** to your SQL Server database.

## Setting Up the Database

1. **Configure your database connection** in `appsettings.json`.

2. **Create the database and tables** if they don't exist. You can use Entity Framework migrations to create the database and apply any changes to the schema.
