using System;

namespace learnova.LearningService.Application.Commands.Topics
{
    public class UpdateTopicCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
