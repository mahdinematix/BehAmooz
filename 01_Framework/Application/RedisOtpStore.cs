using StackExchange.Redis;

namespace _01_Framework.Application
{
    public class RedisOtpStore : IOtpStore
    {
        private readonly IDatabase _database;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(2);
        private readonly TimeSpan _resetPassExpiration = TimeSpan.FromMinutes(5);

        public RedisOtpStore(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task StoreAsync(string phone, int type, string code)
        {
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(code))
                return;

            var key = GetRedisKey(phone, type);
            await _database.StringSetAsync(key, code, _defaultExpiration);
        }

        public async Task<string?> GetAsync(string phone, int type)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            var key = GetRedisKey(phone, type);
            var value = await _database.StringGetAsync(key);

            return value.HasValue ? value.ToString() : null;
        }

        public async Task<int> GetOtpRemainingSecondsAsync(string phoneNumber, int type)
        {
            var key = GetRedisKey(phoneNumber, type);
            var ttl = await _database.KeyTimeToLiveAsync(key);
            return ttl.HasValue ? (int)ttl.Value.TotalSeconds : 0;
        }
        public async Task StoreTokenAsync(string key, string token)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(token))
                return;
            await _database.StringSetAsync(key, token, _resetPassExpiration);
        }

        public async Task<string?> GetTokenAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var value = await _database.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task RemoveTokenAsync(string key)
        {

            if (string.IsNullOrEmpty(key))
                return;

            await _database.KeyDeleteAsync(key); 

        }

        private string GetRedisKey(string phone, int type)
        {
            return $"otp:{type.ToString().ToLower()}:{phone}";
        }
    }
}
