using System;

namespace learnova.LearningService.Persistence.Documents
{
    internal class TopicDocument
    {
        public string id { get; set; } = null!;
        public string UnitId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
