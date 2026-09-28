using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Resources;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Resources;

namespace learnova.LearningService.Application.Handlers.Resources
{
    public class GetLearningResourceByIdHandler
    {
        private readonly ILearningResourceRepository _resourceRepository;

        public GetLearningResourceByIdHandler(ILearningResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<LearningResourceDto?> HandleAsync(GetLearningResourceByIdQuery query, CancellationToken cancellationToken = default)
        {
            var resource = await _resourceRepository.GetByIdAsync(query.Id, cancellationToken);
            if (resource == null) return null;

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
