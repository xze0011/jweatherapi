![Weather Finder API](Weather_Finder.png)

## Features

- Retrieve real-time weather information based on the input city and country
- Implement rate limiting for API requests using a token bucket strategy
- Return well-designed and user-friendly responses to the client

## Tech

Weather Finder API uses a number of technologies to function effectively:

- **[ASP.NET Core 8.0]** - A robust framework for building scalable and high-performance web APIs
- **[Docker]** - Containerization for consistent and portable deployment across environments
- **[Postman]** - Tool for API testing and debugging to ensure reliable performance
- **[Xunit]** - A testing framework for unit tests, ensuring code quality and stability
- **[FakeItEasy]** - A mocking library to simplify testing by creating fake dependencies

## How to Run

This .NET API requires the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) to be installed.

The Weather Finder API can be easily run and tested in Visual Studio IDE or from the command line.

### Configure API Keys

Before running the application, replace the placeholder API keys in `appsettings.json` with your own OpenWeatherMap API keys. This ensures that each user has a unique key and follows best practices for secure key management.

1. Open the `appsettings.json` file.
2. Locate the `OpenWeatherMap` section and replace the placeholder API keys as shown below:

   ```json
   "OpenWeatherMap": {
       "ApiKeys": [
           "YOUR_API_KEY_1",
           "YOUR_API_KEY_2"
       ]
   }
   ```

3. **Note**: Avoid committing sensitive keys directly to source control. For production environments, consider using `appsettings.Production.json`, setting environment variables in the Dockerfile, or securely managing API keys with a secrets manager (e.g., Azure Key Vault).

### Build/Run with Docker(Preconditions)

Using Docker is highly recommended, especially if you’re running both the frontend and backend on the same machine. Docker allows you to set a consistent environment and port number, making it easier to avoid conflicts.

```sh
docker build -t weatherapi .
docker run -p 5000:5000 weatherapi
```

### Running from Command Line

If you prefer to use the command line, you can follow these steps:

```sh
# Navigate to the project directory
cd WeatherApi

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

### Running in Visual Studio

1. Open the solution file (`WeatherApi.sln`) in Visual Studio.
2. Select the **WeatherApi** project as the startup project.
3. Press **F5** or click **Start Debugging** to build and run the application.

## How to Run Tests

Once the API is successfully configured and running, you can proceed to run the test suite to verify that all functionalities are working correctly.

### Running Tests in Visual Studio

1. Open the solution file (`WeatherApi.sln`) in Visual Studio.
2. Go to **Test** > **Test Explorer** in the menu to open the Test Explorer.
3. In the Test Explorer, click **Run All** to execute all tests in the project.

## License

MIT
