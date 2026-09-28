using Microsoft.Extensions.DependencyInjection;

namespace learnova.LearningService.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register infrastructure services (external integrations) here.
            return services;
        }
    }
}
