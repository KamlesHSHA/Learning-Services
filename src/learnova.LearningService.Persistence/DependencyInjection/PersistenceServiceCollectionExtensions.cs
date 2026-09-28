using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos;
using learnova.LearningService.Persistence.Configuration;
using learnova.LearningService.Persistence.Repositories;

namespace learnova.LearningService.Persistence.DependencyInjection
{
    public static class PersistenceServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Bind Cosmos settings from configuration. No secrets are stored in source control.
            services.Configure<CosmosSettings>(configuration.GetSection("Cosmos"));

            // Validate configuration
            var settings = configuration.GetSection("Cosmos").Get<CosmosSettings>();
            if (settings == null || string.IsNullOrWhiteSpace(settings.Endpoint) || string.IsNullOrWhiteSpace(settings.Key) || string.IsNullOrWhiteSpace(settings.DatabaseName))
            {
                // Fail fast with clear message. Real credentials should be provided via user secrets or environment variables at runtime.
                throw new InvalidOperationException("Cosmos configuration is missing or incomplete. Please configure 'Cosmos:Endpoint', 'Cosmos:Key' and 'Cosmos:DatabaseName'.");
            }

            // Register CosmosClient as singleton
            var cosmosClient = new CosmosClient(settings.Endpoint, settings.Key);
            services.AddSingleton(cosmosClient);

            // Ensure database and containers exist
            EnsureDatabaseAndContainersAsync(cosmosClient, settings.DatabaseName).GetAwaiter().GetResult();

            // Register repositories (scoped)
            services.AddScoped<ICourseRepository>(sp => new CourseRepository(cosmosClient, settings.DatabaseName, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CourseRepository>>()));
            services.AddScoped<ISubjectRepository>(sp => new SubjectRepository(cosmosClient, settings.DatabaseName, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<SubjectRepository>>()));
            services.AddScoped<IUnitRepository>(sp => new UnitRepository(cosmosClient, settings.DatabaseName, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<UnitRepository>>()));
            services.AddScoped<ITopicRepository>(sp => new TopicRepository(cosmosClient, settings.DatabaseName, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<TopicRepository>>()));
            services.AddScoped<ILearningResourceRepository>(sp => new LearningResourceRepository(cosmosClient, settings.DatabaseName, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<LearningResourceRepository>>()));
            services.AddScoped<IProgressRepository>(sp => new ProgressRepository(cosmosClient, settings.DatabaseName, sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<ProgressRepository>>()));

            return services;
        }

        private static async System.Threading.Tasks.Task EnsureDatabaseAndContainersAsync(CosmosClient client, string databaseName)
        {
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(databaseName);

            var database = databaseResponse.Database;

            await database.CreateContainerIfNotExistsAsync(CosmosContainerNames.Courses, "/id");
            await database.CreateContainerIfNotExistsAsync(CosmosContainerNames.Subjects, "/courseId");
            await database.CreateContainerIfNotExistsAsync(CosmosContainerNames.Units, "/subjectId");
            await database.CreateContainerIfNotExistsAsync(CosmosContainerNames.Topics, "/unitId");
            await database.CreateContainerIfNotExistsAsync(CosmosContainerNames.LearningResources, "/topicId");
            await database.CreateContainerIfNotExistsAsync(CosmosContainerNames.Progress, "/userId");
        }
    }
}
