FindBestProductAI Application


Overview


FindBestProductAI is a web application designed to provide popular attributes for product categories using OpenAI's GPT-3.5-turbo model. The application comprises an ASP.NET Core backend that communicates with the OpenAI API to retrieve and process popular attributes for specified product categories.

Prerequisites

Before running the application, ensure you have the following installed:

.NET 6.0 SDK
Visual Studio or Visual Studio Code
Serilog


Important: Make sure to place the provided secret key in both the appsettings.json and launchSettings.json files.

Setup and Configuration
Clone the Repository:

Clone the repository to your local machine using the following commands:

1. git clone https://github.com/nshukur1999/FindBestProduct.git

2. cd FindBestProduct


Install Dependencies

Navigate to the project directory and restore the required dependencies:

3. dotnet restore


Build the Application

Build the project using the following command:

4. dotnet build


Run the Application

Run the application using the following command:

5. dotnet run


The application will start and listen on http://localhost:5001 by default.

Using the Application
Swagger UI
You can access the Swagger UI to interact with the API at http://localhost:5001/swagger/index.html. This provides an interactive interface to test the endpoints.

API Endpoint
The primary endpoint is /api/Category, which processes categories and fetches popular attributes for them. The expected payload is a JSON array of categories, each containing a list of subcategories. An example payload is as follows:


[
  {
    "CategoryName": "Electronics",
    "SubCategories": [
      {
        "CategoryId": 80,
        "CategoryName": "Laptops"
      },
      {
        "CategoryId": 81,
        "CategoryName": "Smartphones"
      }
    ]
  }
]


Send a POST request to http://localhost:5001/api/Category with the JSON payload as shown above. The API will return a list of popular attributes for each subcategory.

Project Structure
Controllers/CategoryController.cs: Handles HTTP requests and routes them to the appropriate services.
Services/CategoryService.cs: Contains business logic for processing categories.
Services/OpenAIService.cs: Interacts with the OpenAI API to fetch popular attributes.
Models/: Contains the data models used in the application.
Program.cs: Sets up the application, including dependency injection and middleware configuration.


Logging
The application uses Serilog for logging. Logs are output to the console by default. But we can configure additional logging sinks in Program.cs.
