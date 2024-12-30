﻿using UrlShortener.Core.Urls.Add;
using UrlShortener.Core.Urls;

namespace UrlShortener.Tests.TestDoubles
{
    public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
    {
        public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
        {
            Add(shortened.ShortUrl, shortened);

            return Task.CompletedTask;
        }
    }
}