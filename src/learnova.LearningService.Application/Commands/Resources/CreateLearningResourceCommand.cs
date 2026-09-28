using System;
using learnova.LearningService.Domain.Enums;

namespace learnova.LearningService.Application.Commands.Resources
{
    public class CreateLearningResourceCommand
    {
        public Guid TopicId { get; set; }
        public string Title { get; set; } = null!;
        public string ResourceUri { get; set; } = null!;
        public ResourceType ResourceType { get; set; } = ResourceType.Unknown;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
