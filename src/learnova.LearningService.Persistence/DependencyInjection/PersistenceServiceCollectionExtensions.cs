using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using learnova.LearningService.Persistence.Configuration;

namespace learnova.LearningService.Persistence.DependencyInjection
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind Cosmos settings from configuration. No secrets are stored in source control.
            services.Configure<CosmosSettings>(configuration.GetSection("Cosmos"));

            // Register persistence implementations (Cosmos DB repositories) here in future.
            return services;
        }
    }
}
