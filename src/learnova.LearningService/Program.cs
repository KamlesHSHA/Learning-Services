using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using learnova.LearningService.Application.DependencyInjection;
using learnova.LearningService.Infrastructure.DependencyInjection;
using learnova.LearningService.Persistence.DependencyInjection;

namespace learnova.LearningService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add configuration and services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            // Swagger/OpenAPI will be enabled when Swashbuckle is added to the project.
            // builder.Services.AddSwaggerGen();

            // Application layer
            builder.Services.AddApplication();

            // Infrastructure and Persistence layers - pass configuration when required
            builder.Services.AddInfrastructure();
            builder.Services.AddPersistence(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Enable Swagger UI when Swashbuckle.AspNetCore is added.
                // app.UseSwagger();
                // app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
