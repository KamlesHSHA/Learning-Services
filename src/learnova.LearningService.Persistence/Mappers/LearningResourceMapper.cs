using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class LearningResourceMapper
    {
        public static LearningResource MapToDomain(LearningResourceDocument d)
        {
            var resource = (LearningResource)Activator.CreateInstance(typeof(LearningResource), true)!;
            var t = typeof(LearningResource);
            t.GetProperty("Id")!.SetValue(resource, Guid.Parse(d.id));
            t.GetProperty("TopicId")!.SetValue(resource, Guid.Parse(d.TopicId));
            t.GetProperty("Title")!.SetValue(resource, d.Title);
            t.GetProperty("Description")!.SetValue(resource, d.Description);
            t.GetProperty("ResourceType")!.SetValue(resource, d.ResourceType);
            t.GetProperty("ResourceUri")!.SetValue(resource, d.ResourceUri);
            t.GetProperty("DisplayOrder")!.SetValue(resource, d.DisplayOrder);
            t.GetProperty("IsPublished")!.SetValue(resource, d.IsPublished);
            return resource;
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
