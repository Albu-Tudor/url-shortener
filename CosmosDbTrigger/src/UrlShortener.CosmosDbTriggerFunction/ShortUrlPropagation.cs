using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UrlShortener.CosmosDbTriggerFunction
{
    public class ShortUrlPropagation
    {
        private readonly ILogger _logger;
        private readonly Container _container;

        public ShortUrlPropagation(ILoggerFactory loggerFactory, Container container)
        {
            _logger = loggerFactory.CreateLogger<ShortUrlPropagation>();
            _container = container;
        }

        [Function("ShortUrlPropagation")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "urls",
            containerName: "items",
            Connection = "CosmosDbConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<UrlDocument> input)
        {
            if (input == null && input.Count <= 0) return;

            foreach (var document in input)
            {
                _logger.LogInformation("Short Url: {ShortUrl}", document.Id);

                try
                {
                    await _container.UpsertItemAsync(document, new PartitionKey(document.CreatedBy));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error writing to Cosmos DB");
                    throw;
                }
            }
        }
    }

    public class UrlDocument
    {
        public string Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LongUrl { get; set; }

    }
}
