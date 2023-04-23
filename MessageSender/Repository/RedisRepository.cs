using System.Text.Json;
using MessageSender.Repository;
using StackExchange.Redis;

namespace MessageSender;

public class RedisRepository : IRepository
{
    private readonly IConnectionMultiplexer _redis;

    public RedisRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<bool> Set<T>(string key, T value)
    {
        var database = _redis.GetDatabase();
        var redisKey = new RedisKey(key);

        var serialized = JsonSerializer.Serialize(value);

        var redisValue = new RedisValue(serialized);

        try
        {
            await database.StringSetAsync(redisKey, redisValue);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public async Task<T?> Get<T>(string key)
    {
        var database = _redis.GetDatabase();
        var redisKey = new RedisKey(key);

        try
        {
            var value = await database.StringGetAsync(redisKey);
            var deserialized = JsonSerializer.Deserialize<T>(value);

            return deserialized;
        }
        catch (Exception e)
        {
            return default;
        }
    }

    public async Task Delete(string key)
    {
        var database = _redis.GetDatabase();
        var redisKey = new RedisKey(key);
        await database.KeyDeleteAsync(redisKey);
    }
}