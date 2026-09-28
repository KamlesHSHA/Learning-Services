using System;
using learnova.LearningService.Domain.Exceptions;

namespace learnova.LearningService.Domain.Entities
{
    public class Topic
    {
        public Guid Id { get; private set; }
        public Guid UnitId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        private Topic() { }

        public Topic(Guid unitId, string title, string? description = null, int displayOrder = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Topic title cannot be empty.");

            Id = Guid.NewGuid();
            UnitId = unitId;
            Title = title.Trim();
            Description = description?.Trim();
            DisplayOrder = displayOrder;
            IsActive = true;
        }

        // Persistence factory for reconstructing Topic
        internal static Topic Rehydrate(Guid id, Guid unitId, string title, string? description, int displayOrder, bool isActive)
        {
            var t = new Topic()
            {
                Id = id,
                UnitId = unitId,
                Title = title,
                Description = description,
                DisplayOrder = displayOrder,
                IsActive = isActive
            };
            return t;
        }

        public void Update(string title, string? description, int displayOrder)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Topic title cannot be empty.");

            Title = title.Trim();
            Description = description?.Trim();
            DisplayOrder = displayOrder;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
    }
}
