using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Resources;
using learnova.LearningService.Application.DTOs.Resources;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Handlers.Resources
{
    public class CreateLearningResourceHandler
    {
        private readonly ILearningResourceRepository _resourceRepository;

        public CreateLearningResourceHandler(ILearningResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<LearningResourceDto> HandleAsync(CreateLearningResourceCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));
            if (string.IsNullOrWhiteSpace(command.Title)) throw new System.ArgumentException("Title is required.", nameof(command.Title));
            if (string.IsNullOrWhiteSpace(command.ResourceUri)) throw new System.ArgumentException("ResourceUri is required.", nameof(command.ResourceUri));

            var resource = new LearningResource(command.TopicId, command.Title, command.ResourceUri, command.ResourceType, command.Description, command.DisplayOrder);

            await _resourceRepository.AddAsync(resource, cancellationToken);

            return new LearningResourceDto
            {
                Id = resource.Id,
                TopicId = resource.TopicId,
                Title = resource.Title,
                Description = resource.Description,
                ResourceType = resource.ResourceType,
                ResourceUri = resource.ResourceUri,
                DisplayOrder = resource.DisplayOrder,
                IsPublished = resource.IsPublished
            };
        }
    }
}
