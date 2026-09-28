using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class TopicMapper
    {
        public static Topic MapToDomain(TopicDocument d)
        {
            return Topic.Rehydrate(
                Guid.Parse(d.id),
                Guid.Parse(d.UnitId),
                d.Title,
                d.Description,
                d.DisplayOrder,
                d.IsActive
            );
        }

        public static TopicDocument MapToDocument(Topic domain)
        {
            return new TopicDocument
            {
                id = domain.Id.ToString(),
                UnitId = domain.UnitId.ToString(),
                Title = domain.Title,
                Description = domain.Description,
                DisplayOrder = domain.DisplayOrder,
                IsActive = domain.IsActive
            };
        }
    }
}
