using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class LearningResourceMapper
    {
        public static LearningResource MapToDomain(LearningResourceDocument d)
        {
            return LearningResource.Rehydrate(
                Guid.Parse(d.id),
                Guid.Parse(d.TopicId),
                d.Title,
                d.Description,
                d.ResourceType,
                d.ResourceUri,
                d.DisplayOrder,
                d.IsPublished
            );
        }

        public static LearningResourceDocument MapToDocument(LearningResource domain)
        {
            return new LearningResourceDocument
            {
                id = domain.Id.ToString(),
                TopicId = domain.TopicId.ToString(),
                Title = domain.Title,
                Description = domain.Description,
                ResourceType = domain.ResourceType,
                ResourceUri = domain.ResourceUri,
                DisplayOrder = domain.DisplayOrder,
                IsPublished = domain.IsPublished
            };
        }
    }
}
