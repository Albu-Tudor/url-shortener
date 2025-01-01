using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

using UrlShortener.Libraries.Testing;
using UrlShortener.RedirectApi.Tests.TestDoubles;

namespace UrlShortener.RedirectApi.Tests
{
    public class ApiFixture : WebApplicationFactory<IRedirectApiAssemblyMarker>
    {
        public InMemoryShortenedUrlReader ShortenedUrlReader { get; } = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(
                services =>
                {
                    services.Remove<IShortenerUrlReader>();
                    services.AddSingleton<IShortenerUrlReader>(provider =>
                        ShortenedUrlReader);
                });

            base.ConfigureWebHost(builder);
        }
    }
}
