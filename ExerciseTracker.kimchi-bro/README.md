### Setup
- Define preferred server and database name editing `appsettings.json` file's connection string in `ExerciseTrackerAPI` project in `"DefaultConnection"` property
    - Default values: `Server=(localdb)\\MSSQLLocalDB;` `Database=Exercises;`
- Specify preferred repository in `appsettings.json` file's `"Repositories"`property
    - Default value for EF Core `""`, use `"AdoNet"` for ADO.NET or `"Dapper"` for Dapper repository
- A new database will be created the first time you run the app
- Visit `https://localhost:7203/scalar/` to test API
### Features
- Spectre.Console UI
- Scalar API Client
- MS SQL Server
- FluentMigrator migrations
- Entity Framework Core, Dapper and ADO.NET repositories
- Code-first approach for database creation
### Migrations
- For database migrations see `FluentMigrator` documentation:
    https://fluentmigrator.github.io/articles/intro.html