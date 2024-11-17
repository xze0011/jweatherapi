namespace WeatherApi.Models
{
    public class WeatherResponse
    {
        // Success with the weather description.
        public string? Description { get; set; }
        // Error message if the request failed.
        public string? ErrorMessage { get; set; }
    }
}
