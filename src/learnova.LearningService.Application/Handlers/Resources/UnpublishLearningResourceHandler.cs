using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Resources;
using learnova.LearningService.Application.DTOs.Resources;
using learnova.LearningService.Application.Interfaces;

namespace learnova.LearningService.Application.Handlers.Resources
{
    public class UnpublishLearningResourceHandler
    {
        private readonly ILearningResourceRepository _resourceRepository;

        public UnpublishLearningResourceHandler(ILearningResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<LearningResourceDto> HandleAsync(UnpublishLearningResourceCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));

            var resource = await _resourceRepository.GetByIdAsync(command.Id, cancellationToken)
                           ?? throw new System.InvalidOperationException("Resource not found.");

            resource.Unpublish();

            await _resourceRepository.UpdateAsync(resource, cancellationToken);

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
