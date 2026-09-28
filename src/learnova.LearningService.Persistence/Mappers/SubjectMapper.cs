using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class SubjectMapper
    {
        public static Subject MapToDomain(SubjectDocument d)
        {
            return Subject.Rehydrate(
                Guid.Parse(d.id),
                Guid.Parse(d.CourseId),
                d.Name,
                d.Description,
                d.DisplayOrder,
                d.IsActive
            );
        }

        public static SubjectDocument MapToDocument(Subject domain)
        {
            return new SubjectDocument
            {
                id = domain.Id.ToString(),
                CourseId = domain.CourseId.ToString(),
                Name = domain.Name,
                Description = domain.Description,
                DisplayOrder = domain.DisplayOrder,
                IsActive = domain.IsActive
            };
        }
    }
}
