﻿using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

using UrlShortener.Api;
using UrlShortener.Core;

public class TokenManager : IHostedService
{
    private readonly ITokenRangeApiClient _client;
    private readonly ILogger<TokenManager> _logger;
    private readonly string _machineIdentifier;
    private readonly TokenProvider _tokenProvider;
    private readonly IEnvironmentManager _environmentManager;

    public TokenManager(ITokenRangeApiClient client, TokenProvider tokenProvider, ILogger<TokenManager> logger, IEnvironmentManager environmentManager)
    {
        _client = client;
        _tokenProvider = tokenProvider;
        _logger = logger;
        _machineIdentifier = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") ?? "unknown";
        _environmentManager = environmentManager;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting token manager");

            _tokenProvider.ReachingRangeLimit += async (sender, args) =>
            {
                await AssignNewRangeAsync(cancellationToken);
            };

            await AssignNewRangeAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Token Manager failed to start due to an error.");
            _environmentManager.FatalError(); // Stop the application with a fatal error
        }
    }

    private async Task AssignNewRangeAsync(CancellationToken cancellationToken)
    {
        var range = await _client.AssignRangeAsync(_machineIdentifier, cancellationToken);

        if (range is null)
        {
            throw new Exception("No tokens assigned");
        }

        _tokenProvider.AssignRange(range!);
        _logger.LogInformation("Assigned range: {Start}-{End}", range!.Start, range!.End);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("'Stopping token manager");
        return Task.CompletedTask;
    }
}

public interface ITokenRangeApiClient
{
    Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken cancellationToken);
}

public class TokenRangeApiClient : ITokenRangeApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =
        HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

    public TokenRangeApiClient(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("TokenRangeService");
    }

    public async Task<TokenRange?> AssignRangeAsync(string machineKey, CancellationToken cancellationToken)
    {

        var response = await RetryPolicy.ExecuteAsync(() => 
            _httpClient.PostAsJsonAsync("assign",
                new { Key = machineKey }, cancellationToken));

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to assign new token range");
        }

        var range = await response.Content
            .ReadFromJsonAsync<TokenRange>(cancellationToken: cancellationToken);

        return range;
    }
}
