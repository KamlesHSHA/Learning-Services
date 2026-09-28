using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class CourseMapper
    {
        public static Course MapToDomain(CourseDocument d)
        {
            return Course.Rehydrate(
                Guid.Parse(d.id),
                d.Title,
                d.Description,
                d.Slug,
                d.IsPublished,
                d.CreatedAt,
                d.UpdatedAt
            );
        }

        public static CourseDocument MapToDocument(Course domain)
        {
            return new CourseDocument
            {
                id = domain.Id.ToString(),
                Title = domain.Title,
                Description = domain.Description,
                Slug = domain.Slug,
                IsPublished = domain.IsPublished,
                CreatedAt = domain.CreatedAt,
                UpdatedAt = domain.UpdatedAt
            };
        }
    }
}
