using System;

namespace learnova.LearningService.Persistence.Documents
{
    internal class ProgressDocument
    {
        public string id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? CourseId { get; set; }
        public string? SubjectId { get; set; }
        public string? UnitId { get; set; }
        public string? TopicId { get; set; }
        public string? ResourceId { get; set; }
        public double CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
