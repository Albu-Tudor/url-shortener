using Microsoft.Extensions.Time.Testing;

using UrlShortener.Api.Core.Tests.TestDoubles;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Core.Tests.Urls
{
    public class AddUrlScenarios
    {
        private readonly FakeTimeProvider _timeProvider;
        private readonly AddUrlHandler _handler;
        private readonly InMemoryUrlDataStore _urlDataStore = new();

        public AddUrlScenarios()
        {
            var tokenProvider = new TokenProvider();
            tokenProvider.AssignRange(1, 5);
            var shortUrlGenerator = new ShortUrlGenerator(tokenProvider);
            _timeProvider = new FakeTimeProvider();
            _handler = new AddUrlHandler(shortUrlGenerator, _urlDataStore, _timeProvider);
        }

        [Fact]
        public async Task Should_return_shortened_url()
        {
            var request = CreateUrlRequest();

            var response = await _handler.HandleAsync(request, default);

            response.ShortUrl.Should().NotBeEmpty();
            response.ShortUrl.Should().Be("1");
        }

        [Fact]
        public async Task Should_save_short_url()
        {
            var request = CreateUrlRequest();

            var response = await _handler.HandleAsync(request, default);

            _urlDataStore.Should().ContainKey(response.ShortUrl);
        }

        [Fact]
        public async Task Should_save_short_url_with_created_by_and_created_on()
        {
            var request = CreateUrlRequest();

            var response = await _handler.HandleAsync(request, default);

            _urlDataStore.Should().ContainKey(response.ShortUrl);
            _urlDataStore[response.ShortUrl].CreatedBy.Should().Be(request.CreatedBy);
            _urlDataStore[response.ShortUrl].CreatedOn.Should()
                .Be(_timeProvider.GetLocalNow());
        }

        private static AddUrlRequest CreateUrlRequest()
        {
            return new AddUrlRequest(
                new Uri("https://dometrain.com/"),
                "tudor@gmail.com");
        }
    }
}
