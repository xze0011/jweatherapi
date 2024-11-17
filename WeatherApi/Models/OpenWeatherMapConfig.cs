namespace WeatherApi.Models
{
    public class OpenWeatherMapConfig
    {
        /// The base URL for the OpenWeatherMap API.
        public string BaseUrl { get; set; } = string.Empty;
        /// 2 API keys for accessing the API.
        public List<string> ApiKeys { get; set; } = new List<string>();
    }
}
