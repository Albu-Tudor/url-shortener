using FluentAssertions;

using System.Net;
using System.Net.Http.Json;

using UrlShortener.Core.Urls.Add;
using UrlShortener.Core.Urls.List;

namespace UrlShortener.Tests
{
    [Collection("Api collection")]
    public class ListUrlsFeature
    {
        const string urlsEndpoint = "/api/urls";
        private readonly HttpClient _client;

        public ListUrlsFeature(ApiFixture fixture)
        {
            _client = fixture.CreateClient();
        }

        [Fact]
        public async Task Should_return_200_ok_with_list_of_urls()
        {
            await AddUrl();

            var response = await _client.GetAsync(urlsEndpoint);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var urls = await response.Content
                .ReadFromJsonAsync<ListUrlsResponse>();
            urls.Urls.Should().NotBeEmpty();
        }

        private async Task<AddUrlResponse?> AddUrl(string? url = null)
        {
            url ??= $"https://{Guid.NewGuid()}.tests";

            var response = await _client.PostAsJsonAsync(urlsEndpoint,
                            new AddUrlRequest(new Uri(url), "tudor.albu@gmail.com"));
            
            return await response.Content.ReadFromJsonAsync<AddUrlResponse>();
        }

        [Fact]
        public async Task Should_return_url_when_created_first()
        {
            var urlCreated = await AddUrl("https://testing-in-list.tests");

            var getResponse = await _client.GetAsync(urlsEndpoint);
            var urls = await getResponse.Content
                .ReadFromJsonAsync<ListUrlsResponse>();

            urls.Urls.Should()
                .Contain(url => url.ShortUrl == urlCreated.ShortUrl);
        }
    }
}
