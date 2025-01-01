
using StackExchange.Redis;

namespace UrlShortener.RedirectApi.Infrastructure
{
    public class RedisUrlReader : IShortenerUrlReader
    {
        private readonly IShortenerUrlReader _reader;
        private readonly IDatabase _cache;

        public RedisUrlReader(IShortenerUrlReader reader, IConnectionMultiplexer redis)
        {
            _reader = reader;
            _cache = redis.GetDatabase();
        }

        public async Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
        {
            var cacheUrl = await _cache.StringGetAsync(shortUrl);
            if (cacheUrl.HasValue)
                return new ReadLongUrlResponse(true, cacheUrl.ToString());

            var getUrlResponse = await _reader.GetLongUrlAsync(shortUrl, cancellationToken);

            if (!getUrlResponse.Found)
                return getUrlResponse;

            await _cache.StringSetAsync(shortUrl, getUrlResponse.LongUrl);

            return getUrlResponse;
        }
    }
}
