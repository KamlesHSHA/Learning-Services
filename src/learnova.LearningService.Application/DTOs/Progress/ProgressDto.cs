using System;

namespace learnova.LearningService.Application.DTOs.Progress
{
    public class ProgressDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public Guid? CourseId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? UnitId { get; set; }
        public Guid? TopicId { get; set; }
        public Guid? ResourceId { get; set; }
        public double CompletionPercentage { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
