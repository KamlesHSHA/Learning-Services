using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Topics;
using learnova.LearningService.Application.DTOs.Topics;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Handlers.Topics
{
    public class CreateTopicHandler
    {
        private readonly ITopicRepository _topicRepository;

        public CreateTopicHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<TopicDto> HandleAsync(CreateTopicCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));
            if (string.IsNullOrWhiteSpace(command.Title)) throw new System.ArgumentException("Title is required.", nameof(command.Title));

            var topic = new Topic(command.UnitId, command.Title, command.Description, command.DisplayOrder);

            await _topicRepository.AddAsync(topic, cancellationToken);

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
