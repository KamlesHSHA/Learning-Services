using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Resources;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Resources;

namespace learnova.LearningService.Application.Handlers.Resources
{
    public class GetResourcesByTopicHandler
    {
        private readonly ILearningResourceRepository _resourceRepository;

        public GetResourcesByTopicHandler(ILearningResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        public async Task<IEnumerable<LearningResourceDto>> HandleAsync(GetResourcesByTopicQuery query, CancellationToken cancellationToken = default)
        {
            var items = await _resourceRepository.GetByTopicIdAsync(query.TopicId, cancellationToken);
            return items.Select(r => new LearningResourceDto
            {
                Id = r.Id,
                TopicId = r.TopicId,
                Title = r.Title,
                Description = r.Description,
                ResourceType = r.ResourceType,
                ResourceUri = r.ResourceUri,
                DisplayOrder = r.DisplayOrder,
                IsPublished = r.IsPublished
            });
        }
    }
}
