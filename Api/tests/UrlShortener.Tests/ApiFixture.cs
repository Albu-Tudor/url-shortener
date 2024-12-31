using Microsoft.AspNetCore.Mvc.Testing;

using UrlShortener.Api;
using UrlShortener.Core.Urls.Add;
using Microsoft.AspNetCore.Hosting;
using UrlShortener.Tests.Extentions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Tests.TestDoubles;

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

                    services.Remove<ITokenRangeApiClient>();
                    services.AddSingleton<ITokenRangeApiClient, FakeTokenRangeApiClient>();
                });

            base.ConfigureWebHost(builder);
        }
    }
}
