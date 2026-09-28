using System;

namespace learnova.LearningService.Persistence.Documents
{
    internal class CourseDocument
    {
        public string id { get; set; } = null!; // Cosmos id
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Slug { get; set; } = null!;
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
