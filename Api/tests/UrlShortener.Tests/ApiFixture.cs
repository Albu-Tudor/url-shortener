using Microsoft.AspNetCore.Mvc.Testing;

using UrlShortener.Api;
using UrlShortener.Core.Urls.Add;
using UrlShortener.Core.Urls;
using Microsoft.AspNetCore.Hosting;
using UrlShortener.Tests.Extentions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace UrlShortener.Tests
{
    public class ApiFixture : WebApplicationFactory<IApiAssemblyMarker>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(
                services =>
                {
                    services.Remove<IUrlDataStore>();
                    services
                        .AddSingleton<IUrlDataStore>(
                        new InMemoryUrlDataStore());
                });

            base.ConfigureWebHost(builder);
        }
    }
    public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
    {
        public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
        {
            Add(shortened.ShortUrl, shortened);

            return Task.CompletedTask;
        }
    }
}
