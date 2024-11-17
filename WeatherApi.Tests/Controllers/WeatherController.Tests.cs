using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Controllers;
using WeatherApi.Interfaces;
using Microsoft.Extensions.Logging;
using WeatherApi.Models;

namespace WeatherApi.Tests.Controllers
{
    public class WeatherControllerTests
    {
        private readonly WeatherController _controller;
        private readonly IWeatherService _weatherService;
        private readonly IRateLimitService _rateLimitService;
        private readonly ILocationValidationService _locationValidationService;

        public WeatherControllerTests()
        {
            // Arrange dependencies and initialize the controller
            _weatherService = A.Fake<IWeatherService>();
            _rateLimitService = A.Fake<IRateLimitService>();
            _locationValidationService = A.Fake<ILocationValidationService>();
            var logger = A.Fake<ILogger<WeatherController>>();

            _controller = new WeatherController(
                _weatherService,
                _rateLimitService,
                _locationValidationService,
                logger);
        }

        [Fact]
        public async Task GetWeather_ReturnsOkResult_WhenAllValid()
        {
            // Arrange
            var city = "ValidCity";
            var country = "ValidCountry";
            string? errorMessage;

            // Mock the location validation to succeed
            A.CallTo(() => _locationValidationService.ValidateLocation(city, country, out errorMessage))
                .Returns(true);

            // Mock the rate limit service to allow the request
            A.CallTo(() => _rateLimitService.TryConsumeToken())
                .Returns(true);

            // Mock the weather service to return a successful response
            var weatherResponse = new WeatherResponse
            {
                Description = "Sunny",
                ErrorMessage = null
            };

            A.CallTo(() => _weatherService.GetWeatherDescriptionAsync(city, country))
                .Returns(Task.FromResult(weatherResponse));

            // Act
            var result = await _controller.GetWeather(city, country);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetWeather_ReturnsBadRequest_WhenLocationInvalid()
        {
            // Arrange
            var city = "InvalidCity";
            var country = "InvalidCountry";
            var errorMessage = "Invalid location";
            string? outErrorMessage;

            // Mock the location validation to fail with an error message
            A.CallTo(() => _locationValidationService.ValidateLocation(city, country, out outErrorMessage))
                .Returns(false)
                .AssignsOutAndRefParameters(errorMessage);

            // Act
            var result = await _controller.GetWeather(city, country);

            // Assert
            var badRequestResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badRequestResult.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task GetWeather_ReturnsTooManyRequests_WhenRateLimitExceeded()
        {
            // Arrange
            var city = "ValidCity";
            var country = "ValidCountry";
            string? errorMessage;

            // Mock the location validation to succeed
            A.CallTo(() => _locationValidationService.ValidateLocation(city, country, out errorMessage))
                .Returns(true);

            // Mock the rate limit service to indicate the rate limit has been exceeded
            A.CallTo(() => _rateLimitService.TryConsumeToken())
                .Returns(false);

            // Act
            var result = await _controller.GetWeather(city, country);

            // Assert
            var statusCodeResult = result.Should().BeOfType<ObjectResult>().Subject;
            statusCodeResult.StatusCode.Should().Be(429);
        }
    }
}
