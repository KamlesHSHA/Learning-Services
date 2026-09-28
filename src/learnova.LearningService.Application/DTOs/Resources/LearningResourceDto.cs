using System;
using learnova.LearningService.Domain.Enums;

namespace learnova.LearningService.Application.DTOs.Resources
{
    public class LearningResourceDto
    {
        public Guid Id { get; set; }
        public Guid TopicId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceUri { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
    }
}
