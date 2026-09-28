using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Topics;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Topics;

namespace learnova.LearningService.Application.Handlers.Topics
{
    public class GetTopicByIdHandler
    {
        private readonly ITopicRepository _topicRepository;

        public GetTopicByIdHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<TopicDto?> HandleAsync(GetTopicByIdQuery query, CancellationToken cancellationToken = default)
        {
            var topic = await _topicRepository.GetByIdAsync(query.Id, cancellationToken);
            if (topic == null) return null;

            return new TopicDto
            {
                Id = topic.Id,
                UnitId = topic.UnitId,
                Title = topic.Title,
                Description = topic.Description,
                DisplayOrder = topic.DisplayOrder,
                IsActive = topic.IsActive
            };
        }
    }
}
