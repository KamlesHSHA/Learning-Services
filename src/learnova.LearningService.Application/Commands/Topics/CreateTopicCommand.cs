using System;

namespace learnova.LearningService.Application.Commands.Topics
{
    public class CreateTopicCommand
    {
        public Guid UnitId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
