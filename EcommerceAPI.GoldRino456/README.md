# Ecommerce API Demo

A locally hosted API simulating the back-end of an online games and digital software distributor. This project was created with the goal of better understanding Rest API design and applying it to a real world business use case.
## Features

- Interfaces asynchronously with a SQL Server to create, store, and retrieve game product data, category information, and transaction details.
- Product and sale endpoints contain optional query parameters to filter through database results and use pagination to return a limited number of results per request.
- Game product and sales services are provided to API controllers by Dependency Injection.



## Tech Stack

**Runtime & Framework:** .NET 8, ASP.NET Core

**Database ORM:** Entity Framework

**Database:** SQL Server

**API Platform**: Postman


## Lessons Learned

- I found this project to be a delightful challenge and a big step up from my last exposure to building out APIs. I still have a lot to learn no doubt, but this project marks the first time I'd say I feel confident about how things work under the hood of an API. I had to deal with timing issues due to not awaiting the right methods, data formatting issues with some of my data transfer objects, and a rather annoying issue with json files and infinitely looping references to data. If I were to take another shot at this one from scratch, I think if I spent more time planning my endpoints out and how I need to structure data that moves in and outat the beginning, I'd be able to complete it much faster. 

- Postman was a bit confusing for me at first, mostly just because there appears to be so much to it from the jump, but I can really see the value of that tool now that I've reached the end of the project with it. Perhaps the most helpful bit, at least for me, was that I could create randomized entries using data from Postman. It took some tweaking and a bit of custom javascript to make it work with the "many to many" relationships my data has, but the end result was near magical. I was able to test my endpoints super easily this way and quickly populate my tables with random valid values, even random values from other existing objects in my database.
## Acknowledgements

 - [The C# Academy](https://www.thecsharpacademy.com/) - I feel like I'm learning more and more daily because of y'all! Thank you for being such a great and supportive community!
 - [README Editor](https://readme.so/editor)
 - [Folgers Coffee](https://www.folgerscoffee.com)

