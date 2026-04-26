using StackExchange.Redis;

namespace _01_Framework.Application
{
    public class RedisOtpStore : IOtpStore
    {
        private readonly IDatabase _database;
        private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(2);

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
            var key = GetRedisKey(phoneNumber,type);
            var ttl = await _database.KeyTimeToLiveAsync(key);
            return ttl.HasValue ? (int)ttl.Value.TotalSeconds : 0;
        }
        private string GetRedisKey(string phone, int type)
        {
            return $"otp:{type.ToString().ToLower()}:{phone}";
        }
    }
}
