using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class ProgressMapper
    {
        public static Progress MapToDomain(ProgressDocument d)
        {
            var progress = (Progress)Activator.CreateInstance(typeof(Progress), true)!;
            var t = typeof(Progress);
            t.GetProperty("Id")!.SetValue(progress, Guid.Parse(d.id));
            t.GetProperty("UserId")!.SetValue(progress, d.UserId);
            t.GetProperty("CourseId")!.SetValue(progress, string.IsNullOrEmpty(d.CourseId) ? null : Guid.Parse(d.CourseId));
            t.GetProperty("SubjectId")!.SetValue(progress, string.IsNullOrEmpty(d.SubjectId) ? null : Guid.Parse(d.SubjectId));
            t.GetProperty("UnitId")!.SetValue(progress, string.IsNullOrEmpty(d.UnitId) ? null : Guid.Parse(d.UnitId));
            t.GetProperty("TopicId")!.SetValue(progress, string.IsNullOrEmpty(d.TopicId) ? null : Guid.Parse(d.TopicId));
            t.GetProperty("ResourceId")!.SetValue(progress, string.IsNullOrEmpty(d.ResourceId) ? null : Guid.Parse(d.ResourceId));
            t.GetProperty("CompletionPercentage")!.SetValue(progress, d.CompletionPercentage);
            t.GetProperty("IsCompleted")!.SetValue(progress, d.IsCompleted);
            t.GetProperty("LastAccessedAt")!.SetValue(progress, d.LastAccessedAt);
            t.GetProperty("CompletedAt")!.SetValue(progress, d.CompletedAt);
            return progress;
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
