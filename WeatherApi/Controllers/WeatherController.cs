using Microsoft.AspNetCore.Mvc;
using WeatherApi.Interfaces;

namespace WeatherApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {

        private readonly ILocationValidationService _locationValidationService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(

            ILocationValidationService locationValidationService,
            ILogger<WeatherController> logger)
        {
            _locationValidationService = locationValidationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string city, [FromQuery] string country)
        {
            _logger.LogInformation("Received request to get weather for {City}, {Country}", city, country);

            // Validate location input
            if (!_locationValidationService.ValidateLocation(city, country, out var errorMessage))
            {
                _logger.LogWarning("Location validation failed for {City}, {Country}: {ErrorMessage}", city, country, errorMessage);
                return BadRequest(new { error = errorMessage });
            }

            return Ok();
        }
    }
}
