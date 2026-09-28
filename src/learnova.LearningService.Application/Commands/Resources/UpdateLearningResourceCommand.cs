using System;
using learnova.LearningService.Domain.Enums;

namespace learnova.LearningService.Application.Commands.Resources
{
    public class UpdateLearningResourceCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string ResourceUri { get; set; } = null!;
        public ResourceType ResourceType { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
