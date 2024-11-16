namespace WeatherApi.Models
{
    public class TokenBucket
    {
        public List<Token> Tokens { get; set; }

        /// <summary>
        /// Initializes a new TokenBucket with tokens for each provided key.
        /// </summary>
        /// <param name="keys">List of keys for each token.</param>
        /// <param name="capacity">Maximum uses for each token.</param>
        public TokenBucket(List<string> keys, int capacity)
        {
            Tokens = new List<Token>();
            foreach (var key in keys)
            {
                Tokens.Add(new Token(key, capacity));
            }
        }
    }
}
