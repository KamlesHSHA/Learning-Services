using System;

namespace learnova.LearningService.Application.DTOs.Topics
{
    public class TopicDto
    {
        public Guid Id { get; set; }
        public Guid UnitId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
