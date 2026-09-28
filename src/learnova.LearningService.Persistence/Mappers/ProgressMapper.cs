using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class ProgressMapper
    {
        public static Progress MapToDomain(ProgressDocument d)
        {
            return Progress.Rehydrate(
                Guid.Parse(d.id),
                d.UserId,
                string.IsNullOrEmpty(d.CourseId) ? (Guid?)null : Guid.Parse(d.CourseId),
                string.IsNullOrEmpty(d.SubjectId) ? (Guid?)null : Guid.Parse(d.SubjectId),
                string.IsNullOrEmpty(d.UnitId) ? (Guid?)null : Guid.Parse(d.UnitId),
                string.IsNullOrEmpty(d.TopicId) ? (Guid?)null : Guid.Parse(d.TopicId),
                string.IsNullOrEmpty(d.ResourceId) ? (Guid?)null : Guid.Parse(d.ResourceId),
                d.CompletionPercentage,
                d.IsCompleted,
                d.LastAccessedAt,
                d.CompletedAt
            );
        }

        public static ProgressDocument MapToDocument(Progress domain)
        {
            return new ProgressDocument
            {
                id = domain.Id.ToString(),
                UserId = domain.UserId,
                CourseId = domain.CourseId?.ToString(),
                SubjectId = domain.SubjectId?.ToString(),
                UnitId = domain.UnitId?.ToString(),
                TopicId = domain.TopicId?.ToString(),
                ResourceId = domain.ResourceId?.ToString(),
                CompletionPercentage = domain.CompletionPercentage,
                IsCompleted = domain.IsCompleted,
                LastAccessedAt = domain.LastAccessedAt,
                CompletedAt = domain.CompletedAt
            };
        }
    }
}
