namespace WeatherApi.Models
{
    public class RateLimitConfig
    {
        // 5 Client keys for rate limiting.
        public List<string> ClientKeys { get; set; } = new List<string>();

        // 5 Times for each client key.
        public int TokenCapacity { get; set; } = 5;
    }
}
