using System;
using learnova.LearningService.Domain.Exceptions;

namespace learnova.LearningService.Domain.Entities
{
    public class Progress
    {
        public Guid Id { get; private set; }
        public string UserId { get; private set; }
        public Guid? CourseId { get; private set; }
        public Guid? SubjectId { get; private set; }
        public Guid? UnitId { get; private set; }
        public Guid? TopicId { get; private set; }
        public Guid? ResourceId { get; private set; }
        public double CompletionPercentage { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime? LastAccessedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        private Progress() { }

        public Progress(string userId, Guid? courseId = null, Guid? subjectId = null, Guid? unitId = null, Guid? topicId = null, Guid? resourceId = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new DomainException("UserId is required for Progress.");

            Id = Guid.NewGuid();
            UserId = userId;
            CourseId = courseId;
            SubjectId = subjectId;
            UnitId = unitId;
            TopicId = topicId;
            ResourceId = resourceId;
            CompletionPercentage = 0;
            IsCompleted = false;
        }

        // Persistence factory for reconstructing Progress
        internal static Progress Rehydrate(Guid id, string userId, Guid? courseId, Guid? subjectId, Guid? unitId, Guid? topicId, Guid? resourceId, double completionPercentage, bool isCompleted, DateTime? lastAccessedAt, DateTime? completedAt)
        {
            var p = new Progress()
            {
                Id = id,
                UserId = userId,
                CourseId = courseId,
                SubjectId = subjectId,
                UnitId = unitId,
                TopicId = topicId,
                ResourceId = resourceId,
                CompletionPercentage = completionPercentage,
                IsCompleted = isCompleted,
                LastAccessedAt = lastAccessedAt,
                CompletedAt = completedAt
            };
            return p;
        }

        public void UpdateCompletion(double percent)
        {
            if (percent < 0 || percent > 100)
                throw new DomainException("Completion percentage must be between 0 and 100.");

            CompletionPercentage = percent;
            LastAccessedAt = DateTime.UtcNow;
            if (CompletionPercentage >= 100)
                MarkCompleted();
        }

        public void MarkCompleted()
        {
            CompletionPercentage = 100;
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }

        public void RecordAccess()
        {
            LastAccessedAt = DateTime.UtcNow;
        }
    }
}
