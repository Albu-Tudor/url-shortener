﻿using FluentAssertions;

using Microsoft.Extensions.Logging;

using NSubstitute;

using UrlShortener.Api;
using UrlShortener.Core;

namespace UrlShortener.Tests
{
    public class TokenManagerScenarios
    {
        [Fact]
        public async Task Should_call_api_on_start()
        {
            var tokenRangeApiClient = Substitute.For<ITokenRangeApiClient>();
            tokenRangeApiClient.AssignRangeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(new TokenRange(1, 10));
            
            var tokenManager = new TokenManager(tokenRangeApiClient,
                Substitute.For<TokenProvider>(),
                Substitute.For<ILogger<TokenManager>>(),
                Substitute.For<IEnvironmentManager>());

            await tokenManager.StartAsync(default);

            await tokenRangeApiClient.Received().AssignRangeAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Should_throw_exception_when_no_tokens_assigned()
        {
            var tokenRangeApiClient = Substitute.For<ITokenRangeApiClient>();
            var environmentManager = Substitute.For<IEnvironmentManager>();
            var tokenManager = new TokenManager(tokenRangeApiClient,
                Substitute.For<TokenProvider>(),
                Substitute.For<ILogger<TokenManager>>(),
                environmentManager);

            await tokenManager.StartAsync(default);

            environmentManager.Received().FatalError();
        }
    }
}
