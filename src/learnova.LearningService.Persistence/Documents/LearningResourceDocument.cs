using System;
using learnova.LearningService.Domain.Enums;

namespace learnova.LearningService.Persistence.Documents
{
    internal class LearningResourceDocument
    {
        public string id { get; set; } = null!;
        public string TopicId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public ResourceType ResourceType { get; set; }
        public string ResourceUri { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
    }
}
