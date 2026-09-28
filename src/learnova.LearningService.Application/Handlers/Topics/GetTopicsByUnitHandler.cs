using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Topics;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Topics;

namespace learnova.LearningService.Application.Handlers.Topics
{
    public class GetTopicsByUnitHandler
    {
        private readonly ITopicRepository _topicRepository;

        public GetTopicsByUnitHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<IEnumerable<TopicDto>> HandleAsync(GetTopicsByUnitQuery query, CancellationToken cancellationToken = default)
        {
            var items = await _topicRepository.GetByUnitIdAsync(query.UnitId, cancellationToken);
            return items.Select(t => new TopicDto
            {
                Id = t.Id,
                UnitId = t.UnitId,
                Title = t.Title,
                Description = t.Description,
                DisplayOrder = t.DisplayOrder,
                IsActive = t.IsActive
            });
        }
    }
}
