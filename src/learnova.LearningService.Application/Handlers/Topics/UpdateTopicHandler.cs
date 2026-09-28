using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Topics;
using learnova.LearningService.Application.DTOs.Topics;
using learnova.LearningService.Application.Interfaces;

namespace learnova.LearningService.Application.Handlers.Topics
{
    public class UpdateTopicHandler
    {
        private readonly ITopicRepository _topicRepository;

        public UpdateTopicHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<TopicDto> HandleAsync(UpdateTopicCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));

            var topic = await _topicRepository.GetByIdAsync(command.Id, cancellationToken)
                        ?? throw new System.InvalidOperationException("Topic not found.");

            topic.Update(command.Title, command.Description, command.DisplayOrder);

            await _topicRepository.UpdateAsync(topic, cancellationToken);

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
