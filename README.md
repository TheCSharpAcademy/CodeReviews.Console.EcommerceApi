# Ecommerce.API.davetn657

## Overview

A WebAPI for an ecommerce that stores and sends product, sales, and categories data

Made for C# Academey Project based Learning

## Requirements

- Must use ASP.NET Core Web API and EF core
- API need to use Dependency Injection
- Should have atleast three tables (products, sales, categories)
- Products must have a price
- Multiple products can be used in the same sale
- Products and sales get endpoints must have pagination capability

## Technologies

- C#
- Entity Framework Core
- Sqlite

## Looking back

- I feel I am getting more familiar with building projects, planning and actually building them out has gotten significantly easier even with more complex projects
- For this project the only thing I struggled with was the many-to-many relationship between Sales and Products. It was difficult to understand how to implement it as I kept getting cyclical errors whenever I used either GET methods. I found that using Dtos to manage what data is being input/output fixed this for me.
- I was able to build out most of the API without needing to look back at documents and past project.
- This was my first time using pagination and it was mostly copying from the learning resource, but I did need to modify it a little to fit better into my codebase which helped me understand the process.
