using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class UnitMapper
    {
        public static Unit MapToDomain(UnitDocument d)
        {
            return Unit.Rehydrate(
                Guid.Parse(d.id),
                Guid.Parse(d.SubjectId),
                d.Title,
                d.Description,
                d.DisplayOrder,
                d.IsActive
            );
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
