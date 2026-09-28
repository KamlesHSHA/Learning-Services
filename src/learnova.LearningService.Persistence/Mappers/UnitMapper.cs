using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class UnitMapper
    {
        public static Unit MapToDomain(UnitDocument d)
        {
            var unit = (Unit)Activator.CreateInstance(typeof(Unit), true)!;
            var t = typeof(Unit);
            t.GetProperty("Id")!.SetValue(unit, Guid.Parse(d.id));
            t.GetProperty("SubjectId")!.SetValue(unit, Guid.Parse(d.SubjectId));
            t.GetProperty("Title")!.SetValue(unit, d.Title);
            t.GetProperty("Description")!.SetValue(unit, d.Description);
            t.GetProperty("DisplayOrder")!.SetValue(unit, d.DisplayOrder);
            t.GetProperty("IsActive")!.SetValue(unit, d.IsActive);
            return unit;
        }

        public static UnitDocument MapToDocument(Unit domain)
        {
            return new UnitDocument
            {
                id = domain.Id.ToString(),
                SubjectId = domain.SubjectId.ToString(),
                Title = domain.Title,
                Description = domain.Description,
                DisplayOrder = domain.DisplayOrder,
                IsActive = domain.IsActive
            };
        }
    }
}
