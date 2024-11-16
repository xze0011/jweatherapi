using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using WeatherApi.Interfaces;
using WeatherApi.Models;

namespace WeatherApi.Services
{
        public class WeatherService : IWeatherService
        {
            private readonly HttpClient _httpClient;
            private readonly List<string> _apiKeys;
            private readonly string _baseUrl;
            private readonly ILogger<WeatherService> _logger;

            public WeatherService(HttpClient httpClient, IOptions<OpenWeatherMapConfig> openWeatherMapConfig, ILogger<WeatherService> logger)
            {
                _httpClient = httpClient;
                var config = openWeatherMapConfig.Value;
                _apiKeys = config.ApiKeys;
                _baseUrl = config.BaseUrl;
                _logger = logger;
            }

        /// <summary>
        /// Fetches weather data from the OpenWeatherMap API with structured error msg.
        /// <param name="city">The city name.</param>
        /// <param name="country">The country name.</param>
        /// <returns> [WeatherResponse] with the weather description Or an error message.</returns>
        /// </summary>

        public async Task<WeatherResponse> GetWeatherDescriptionAsync(string city, string country)
            {
                _logger.LogInformation("Fetching weather data for {City}, {Country}", city, country);

                if (string.IsNullOrEmpty(_baseUrl))
                {
                    _logger.LogError("Configuration error: Base URL for OpenWeatherMap API is missing.");
                    return new WeatherResponse
                    {
                        ErrorMessage = "Configuration error: Base URL for OpenWeatherMap API is missing."
                    };
                }

                if (_apiKeys == null || !_apiKeys.Any() || _apiKeys.Any(string.IsNullOrEmpty))
                {
                    _logger.LogError("Configuration error: No valid API keys available for OpenWeatherMap.");
                    return new WeatherResponse
                    {
                        ErrorMessage = "Configuration error: No valid API keys available for OpenWeatherMap."
                    };
                }

                // Shunt the API keys capacity, 50% chance for each 
                var random = new Random();
                var selectedApiKey = _apiKeys[random.Next(_apiKeys.Count)];

                var requestUrl = $"{_baseUrl}?q={city},{country}&appid={selectedApiKey}";

                try
                {
                    var response = await _httpClient.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(responseBody);

                // Extract weather description only with error handling
                var description = json["weather"]?[0]?["description"]?.ToString();

                    if (string.IsNullOrEmpty(description))
                    {
                        _logger.LogWarning("Weather description not available from the API for {City}, {Country}", city, country);
                        return new WeatherResponse
                        {
                            ErrorMessage = "Weather description not available from the API."
                        };
                    }

                    _logger.LogInformation("Successfully retrieved weather description for {City}, {Country}: {Description}", city, country, description);
                    return new WeatherResponse
                    {
                        Description = description
                    };
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "Error retrieving data from OpenWeatherMap API for {City}, {Country}", city, country);
                    return new WeatherResponse
                    {
                        ErrorMessage = $"No weather data available for {city}, {country}."
                    };
                }
            }
    }
}
