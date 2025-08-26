# Ecommerce API

The [Ecommerce API](https://www.thecsharpacademy.com/project/18/ecommerce-api) project is an API using SQl Server and Entity Framework. During this project I gained a better understanding of the controller>service>repository architecture and what belongs and doesn't belong in each layer. I also gained a better understanding of how to use Postman including saving requests to a collection and using variable placeholders. This project is part of [The C# Academy](https://www.thecsharpacademy.com/) curriculum.

## Requirements

- [x] Your project needs to be an ASP.NET Core Web API, with Entity Framework and your choice between SQL Server and Sqlite.
- [x] Your API needs to use Dependency Injection.
- [x] You should have at least three tables: Products, Categories and Sales.
- [x] Products and Sales need to have a many-to-many relationship, meaning products can have multiple sales, and sales can have multiple products.
- [x] Products need to have a price. Multiple products can be sold in the same sale.
- [x] You need to provide a Postman Collection with all possible requests for your API. It's a json file that needs to be included in your PR.
- [x] You don't need to create an UI to consume your API.
- [x] Your GetProducts and GetSales endpoints need to have pagination capabilities.
- [x] In retail it's good practice to prevent deletion of records. Feel free to add soft-deletes.

## Challenges
- [x] Add filtering and sorting capabilities to your endpoints. 