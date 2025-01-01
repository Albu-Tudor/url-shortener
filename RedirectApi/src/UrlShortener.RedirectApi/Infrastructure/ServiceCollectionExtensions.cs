using Microsoft.Azure.Cosmos;

namespace UrlShortener.RedirectApi.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUrlReader(this IServiceCollection services,
            string cosmosConnectionString,
            string databaseName, string containerName)
        {
            services.AddSingleton <CosmosClient>(s => 
            new CosmosClient(connectionString: cosmosConnectionString));

            services.AddSingleton<IShortenerUrlReader>(s =>
            {
                var cosmosClient = s.GetRequiredService<CosmosClient>();
                var container = cosmosClient.GetContainer(databaseName, containerName);

                return new CosmosShortenerUrlReader(container);
            });

            return services;
        }
    }
}
