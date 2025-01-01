using UrlShortener.RedirectApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddUrlReader(
    cosmosConnectionString: builder.Configuration["CosmosDb:ConnectionString"]!,
    databaseName: builder.Configuration["DatabaseName"]!,
    containerName: builder.Configuration["ContainerName"]!,
    redisConnectionString: builder.Configuration["Redis:ConnectionString"]!);

app.MapGet("/", () => "Redirect API");

app.MapGet("r/{shortUrl}",
    async (string shortUrl, IShortenerUrlReader reader, CancellationToken cancellationToken) =>
    {
        var response = await reader.GetLongUrlAsync(shortUrl, cancellationToken);

        return response switch
        {
            { Found: true, LongUrl: not null } 
                => Results.Redirect(response.LongUrl, true),
            _ => Results.NotFound()
        };
    });

app.Run();
