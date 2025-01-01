


public interface IShortenerUrlReader
{
    public Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken);
}
