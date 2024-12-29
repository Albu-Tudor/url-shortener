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

            response.Succeded.Should().BeTrue();
            response.Value!.ShortUrl.Should().NotBeEmpty();
            response.Value!.ShortUrl.Should().Be("1");
        }

        [Fact]
        public async Task Should_save_short_url()
        {
            var request = CreateUrlRequest();

            var response = await _handler.HandleAsync(request, default);

            response.Succeded.Should().BeTrue();
            _urlDataStore.Should().ContainKey(response.Value!.ShortUrl);
        }

        [Fact]
        public async Task Should_save_short_url_with_created_by_and_created_on()
        {
            var request = CreateUrlRequest();

            var response = await _handler.HandleAsync(request, default);

            response.Succeded.Should().BeTrue();
            _urlDataStore.Should().ContainKey(response.Value!.ShortUrl);
            _urlDataStore[response.Value!.ShortUrl].CreatedBy.Should().Be(request.CreatedBy);
            _urlDataStore[response.Value!.ShortUrl].CreatedOn.Should()
                .Be(_timeProvider.GetLocalNow());
        }

        [Fact]
        public async Task Should_return_error_if_created_by_us_empty()
        {;
            var request = CreateUrlRequest(createdBy: string.Empty);

            var response = await _handler.HandleAsync(request, default);

            response.Succeded.Should().BeFalse();
            response.Error.Code.Should().Be("missing_value");
        }

        private static AddUrlRequest CreateUrlRequest(string createdBy = "tudor@gmail.com")
        {
            return new AddUrlRequest(
                new Uri("https://dometrain.com/"),
                createdBy);
        }
    }
}
