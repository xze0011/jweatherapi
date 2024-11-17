namespace WeatherApi.Interfaces
{
    public interface IRateLimitService
    {
        bool TryConsumeToken();
    }
}
