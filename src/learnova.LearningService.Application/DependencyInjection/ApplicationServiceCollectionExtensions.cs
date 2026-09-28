using Microsoft.Extensions.DependencyInjection;
using learnova.LearningService.Application.Handlers.Courses;
using learnova.LearningService.Application.Handlers.Progress;

namespace learnova.LearningService.Application.DependencyInjection
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application handlers. Repository implementations are provided by Persistence in later phases.
            services.AddScoped<CreateCourseHandler>();
            services.AddScoped<GetCourseByIdHandler>();
            services.AddScoped<GetCoursesHandler>();
            services.AddScoped<UpdateCourseHandler>();
            services.AddScoped<PublishCourseHandler>();
            services.AddScoped<UnpublishCourseHandler>();

            services.AddScoped<UpdateProgressHandler>();
            services.AddScoped<GetProgressByUserHandler>();

            return services;
        }
    }
}
