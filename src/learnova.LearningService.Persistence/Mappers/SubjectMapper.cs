using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class SubjectMapper
    {
        public static Subject MapToDomain(SubjectDocument d)
        {
            var subj = (Subject)Activator.CreateInstance(typeof(Subject), true)!;
            var t = typeof(Subject);
            t.GetProperty("Id")!.SetValue(subj, Guid.Parse(d.id));
            t.GetProperty("CourseId")!.SetValue(subj, Guid.Parse(d.CourseId));
            t.GetProperty("Name")!.SetValue(subj, d.Name);
            t.GetProperty("Description")!.SetValue(subj, d.Description);
            t.GetProperty("DisplayOrder")!.SetValue(subj, d.DisplayOrder);
            t.GetProperty("IsActive")!.SetValue(subj, d.IsActive);
            return subj;
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
