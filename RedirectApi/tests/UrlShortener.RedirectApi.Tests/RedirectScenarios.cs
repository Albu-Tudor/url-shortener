using Microsoft.AspNetCore.Mvc.Testing;

using UrlShortener.RedirectApi.Tests.TestDoubles;

namespace UrlShortener.RedirectApi.Tests
{
    [Collection("Api collection")]
    public class RedirectScenarios
    {
        public readonly HttpClient _client;
        public readonly InMemoryShortenedUrlReader _storage;

        public RedirectScenarios(ApiFixture fixture)
        {
            _client = fixture.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _storage = fixture.ShortenedUrlReader;
        }

        [Fact]
        public async Task Should_return_301_redirect_with_Url_when_short_url_exists()
        {
            const string shortUrl = "abc123";
            _storage.Add(shortUrl, new ReadLongUrlResponse(true, "https://dometrain.com"));
            
            var response = await _client.GetAsync($"/r/{shortUrl}");

            response.Should().BeRedirection();
            response.Should()
                .HaveStatusCode(System.Net.HttpStatusCode.MovedPermanently);
            response.Headers.Location.Should().Be("https://dometrain.com");
        }

        [Fact]
        public async Task Should_return_404_not_found_when_url_does_not_exists()
        {
            const string shortUrl = "non-existing";

            var response = await _client.GetAsync($"/r/{shortUrl}");

            response.Should()
                .HaveStatusCode(System.Net.HttpStatusCode.NotFound);
        }
    }
}
