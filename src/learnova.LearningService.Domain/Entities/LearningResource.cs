using System;
using learnova.LearningService.Domain.Enums;
using learnova.LearningService.Domain.Exceptions;

namespace learnova.LearningService.Domain.Entities
{
    public class LearningResource
    {
        public Guid Id { get; private set; }
        public Guid TopicId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public ResourceType ResourceType { get; private set; }
        public string ResourceUri { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsPublished { get; private set; }

        private LearningResource() { }

        public LearningResource(Guid topicId, string title, string resourceUri, ResourceType resourceType = ResourceType.Unknown, string? description = null, int displayOrder = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("LearningResource title cannot be empty.");
            if (string.IsNullOrWhiteSpace(resourceUri))
                throw new DomainException("LearningResource must have a resource URI.");

            Id = Guid.NewGuid();
            TopicId = topicId;
            Title = title.Trim();
            Description = description?.Trim();
            ResourceUri = resourceUri.Trim();
            ResourceType = resourceType;
            DisplayOrder = displayOrder;
            IsPublished = false;
        }

        // Persistence factory for reconstructing LearningResource
        internal static LearningResource Rehydrate(Guid id, Guid topicId, string title, string? description, ResourceType resourceType, string resourceUri, int displayOrder, bool isPublished)
        {
            var lr = new LearningResource()
            {
                Id = id,
                TopicId = topicId,
                Title = title,
                Description = description,
                ResourceType = resourceType,
                ResourceUri = resourceUri,
                DisplayOrder = displayOrder,
                IsPublished = isPublished
            };
            return lr;
        }

        public void Update(string title, string? description, string resourceUri, ResourceType resourceType, int displayOrder)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("LearningResource title cannot be empty.");
            if (string.IsNullOrWhiteSpace(resourceUri))
                throw new DomainException("LearningResource must have a resource URI.");

            Title = title.Trim();
            Description = description?.Trim();
            ResourceUri = resourceUri.Trim();
            ResourceType = resourceType;
            DisplayOrder = displayOrder;
        }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Unpublish()
        {
            IsPublished = false;
        }
    }
}
