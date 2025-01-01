
namespace UrlShortener.RedirectApi.Tests.TestDoubles
{
    public class InMemoryShortenedUrlReader : Dictionary<string, ReadLongUrlResponse>, IShortenerUrlReader
    {
        public Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
        {
            return Task.FromResult(ContainsKey(shortUrl) ? this[shortUrl]: new ReadLongUrlResponse(false, null));
        }
    }
}
