using WeatherApi.Interfaces;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class RateLimitService : IRateLimitService
    {
        private readonly TokenBucket _tokenBucket;
        private readonly TimeSpan _refillInterval = TimeSpan.FromHours(1);
        private readonly object _lock = new object();
        private readonly ILogger<RateLimitService> _logger;

        public RateLimitService(TokenBucket tokenBucket, ILogger<RateLimitService> logger)
        {
            _tokenBucket = tokenBucket;
            _logger = logger;
        }

        /// <summary>
        /// Attempts to consume a token time. 
        /// </summary>
        /// <returns>True if a token was consumed; otherwise, false.</returns>
        public bool TryConsumeToken()
        {
            lock (_lock)
            {
                foreach (var token in _tokenBucket.Tokens)
                {
                    // Refill token to max capacity if refill interval(1h) has passed
                    token.Refill(_refillInterval);

                    if (token.RemainingUses > 0)
                    {
                        // Token time -1
                        token.RemainingUses--;
                        _logger.LogInformation("Token consumed. Remaining uses: {RemainingUses}", token.RemainingUses);
                        return true;
                    }
                }
                // No token & token time available
                _logger.LogWarning("Rate limit exceeded. No tokens available.");
                return false;
            }
        }
    }
}
