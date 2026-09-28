using System;

namespace learnova.LearningService.Persistence.Documents
{
    internal class SubjectDocument
    {
        public string id { get; set; } = null!;
        public string CourseId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
