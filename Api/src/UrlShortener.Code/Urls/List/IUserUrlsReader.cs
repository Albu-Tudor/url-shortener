﻿using UrlShortener.Core.Urls.List;

namespace UrlShortener.Core.Urls.Add
{
    public interface IUserUrlsReader
    {
        Task<ListUrlsResponse> GetAsync(string createdBy, CancellationToken cancellationToken);
    }
}
