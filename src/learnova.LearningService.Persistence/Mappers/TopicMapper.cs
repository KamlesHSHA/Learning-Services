using System;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Mappers
{
    internal static class TopicMapper
    {
        public static Topic MapToDomain(TopicDocument d)
        {
            var topic = (Topic)Activator.CreateInstance(typeof(Topic), true)!;
            var t = typeof(Topic);
            t.GetProperty("Id")!.SetValue(topic, Guid.Parse(d.id));
            t.GetProperty("UnitId")!.SetValue(topic, Guid.Parse(d.UnitId));
            t.GetProperty("Title")!.SetValue(topic, d.Title);
            t.GetProperty("Description")!.SetValue(topic, d.Description);
            t.GetProperty("DisplayOrder")!.SetValue(topic, d.DisplayOrder);
            t.GetProperty("IsActive")!.SetValue(topic, d.IsActive);
            return topic;
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
