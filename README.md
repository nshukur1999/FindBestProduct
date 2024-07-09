# FindBestProductAI Application

## Overview

FindBestProductAI is a web application that provides popular attributes for product categories using OpenAI's GPT-3.5-turbo model. This application consists of an ASP.NET Core backend that interacts with the OpenAI API to fetch and process popular attributes for given product categories.

## Prerequisites

Before running the application, ensure you have the following installed:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
- [Serilog](https://serilog.net/)
- An API key from [OpenAI](https://beta.openai.com/signup/)

## Setup and Configuration

 **Clone the Repository**
   
   Clone the repository to your local machine using the following command:

   git clone <repository_url>
   cd FindBestProductAI
   

Install Dependencies

Navigate to the project directory and restore the required dependencies using the following command:


dotnet restore


Build the Application
Build the project using the following command:

dotnet build
Run the Application

Run the application using the following command:

dotnet run
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
Processing Categories

Send a POST request to http://localhost:5001/api/Category with the JSON payload as shown above. The API will return a list of popular attributes for each subcategory.

Project Structure
Controllers/CategoryController.cs: Handles HTTP requests and routes them to the appropriate services.
Services/CategoryService.cs: Contains business logic for processing categories.
Services/OpenAIService.cs: Interacts with the OpenAI API to fetch popular attributes.
Models/: Contains the data models used in the application.
Program.cs: Sets up the application, including dependency injection and middleware configuration.
Logging
The application uses Serilog for logging. Logs are output to the console by default. You can configure additional logging sinks in Program.cs.

Error Handling
The application implements basic error handling and retries for rate limiting when calling the OpenAI API.
Ensure your API key is valid and has the necessary permissions to avoid unauthorized access errors.
