# ECommerce API
An ASP.NET and Entity Framework API, built following the C# Academy specifications found on: https://www.thecsharpacademy.com/project/18/ecommerce-api

This project has a "frontend" in the form of a console application; this repository can be found on: https://github.com/JJHH17/ECommerce-Console 

## Project Overview
This project:
- Use SQL Server for database storage.
- Allows users to Add and Fetch products, categories and sales.
- A single item is a "Product" - a product can have a single category, and a category may have multiple products.
- A sale is a request to order a list of products that exist on the database.
- This project also has a Postman collection which can be found within the "postmanCollection" folder of the project - please install the json file and import it into Postman in order to use it.
- The collection also contains full documentation on the endpoints and supported parameters.

## Endpoint Structure:
- Product = This is an individual item - it contains an ID, name, price - it also contains a one to many relationship with "Category" and a many to many relationship with "Sale".
- Category = This is the type of item that a product is, such as "Nike" or "Adidas" as a brand of shoe - This has a many to one relationship with "Product".
- Sale = This allows the client to create a list of products to execute a sale on. This has a many to many relationship with Product, and will also calculate the overall quantity of items and overall cost of the transaction once a product list is provided.

## Technology Used
- ASP.NET
- SQL Server
- Entity Framework
- Postman
- Swagger / OpenAPI

## Installation steps and requirements
- A localdb instance of SQL Server is needed for this project - I'd recommend following this tutorial for steps on how to create an instance: https://www.youtube.com/watch?v=M5DhHYQlnq8&t=201s&pp=ygUPTE9DQUwgc3Fsc2VydmVy
- Once done, the connection string can be configured within the "DefaultConnection" variable within the projects appsettings.json file.
Once done, run the .sln file and run the project - you'll be directed to Swagger via your default browser where you'll also be able to test the API's endpoints.

## Additional Details
- The project also supports pagination, as well as data seeding once the database instance is created.
- A console based UI can be found and installed via: https://github.com/JJHH17/ECommerce-Console

## Endpoints and usage
- Product endpoints (GET, GET ID, POST) - Allows the client to get a list of, a single, or create individual products.
<img width="170" height="125" alt="image" src="https://github.com/user-attachments/assets/8227bf61-53e1-49e0-a8d8-c331b0807b6e" />

- Sale endpoints (GET, GET ID, POST) - Allows the client to get a list of, a single, or create individual sales.
<img width="170" height="125" alt="image" src="https://github.com/user-attachments/assets/b71e0260-8b05-4d91-b1ea-0ff684ac7bd9" />

- Category endpoints (GET, GET ID, POST) - Allows the client to get a list of, a single, or create individual categories.
<img width="170" height="125" alt="image" src="https://github.com/user-attachments/assets/bc817acc-3018-473b-8b41-0164b84741e3" />


## Key takeaways and learnings from project
This was a great and challenging project to complete - initially, I had a few blockers with understanding how to configure many to many (as well as one to many) relationships within Entity Framework and then mapping them for my API endpoints.

In the end, I was overcomplicating my thoughts and solutions with these, and I decided to create 3 seperate controllers and services to process them, and then mapping each one within the DB Context file - It took around half a week or playing around with different options and solutions to stick to this specific route, although I'm pleased that I did get stuck here, as it was a great learning experienced and allowed me to also lean on Microsoft's documentation more.

I then set about creating the Postman collection which I have some experience with already, although it was very cool to create a collection pointing towards an API that I had created myself, versus consuming a public or third party API which was the only real prior experience I had before.

Finally, creating the console application (found via: https://github.com/JJHH17/ECommerce-Console) was also a great experience - again, I've not had massive amounts of experience consuming my own API that I've created, so it was extremely rewarding on that front - I ensured to create the projects separately; previously, I'd mixed my console app with the backend API, which was causing both apps to mix with eachothers logs, so separating these two project should prevent that in the future.

I'd like to thank, as always, the C# Academy team for the opportunity to work on this project, the Discord community for their inspiration, as well as the team of individuals who review these projects - I've had amazing feedback so far from previous projects which has always been a great and rewarding experience and I look forward to giving back to the same community now that I'm starting to review projects myself.
