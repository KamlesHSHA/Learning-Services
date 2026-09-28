using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class CourseMapper
    {
        public static Course MapToDomain(CourseDocument d)
        {
            var course = (Course)Activator.CreateInstance(typeof(Course), true)!;
            var t = typeof(Course);
            t.GetProperty("Id")!.SetValue(course, Guid.Parse(d.id));
            t.GetProperty("Title")!.SetValue(course, d.Title);
            t.GetProperty("Description")!.SetValue(course, d.Description);
            t.GetProperty("Slug")!.SetValue(course, d.Slug);
            t.GetProperty("IsPublished")!.SetValue(course, d.IsPublished);
            t.GetProperty("CreatedAt")!.SetValue(course, d.CreatedAt);
            t.GetProperty("UpdatedAt")!.SetValue(course, d.UpdatedAt);
            return course;
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
