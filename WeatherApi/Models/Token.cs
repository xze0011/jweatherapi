namespace WeatherApi.Models
{
    public class Token
    {
        public string Key { get; set; }
        public int RemainingUses { get; set; }
        public DateTime LastRefillTime { get; set; }
        private readonly int _capacity;

        public Token(string key, int capacity)
        {
            Key = key;
            RemainingUses = capacity;
            _capacity = capacity;
            LastRefillTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Refills the token to full capacity if the specified interval has passed since [LastRefillTime].
        /// </summary>
        /// <param name="refillInterval">The minimum time interval required between refills.</param>
        public void Refill(TimeSpan refillInterval)
        {
            if (DateTime.UtcNow - LastRefillTime >= refillInterval)
            {
                RemainingUses = _capacity;
                LastRefillTime = DateTime.UtcNow;
            }
        }
    }
}
