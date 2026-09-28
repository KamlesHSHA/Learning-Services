using System;
using System.Collections.Generic;
using System.Linq;
using learnova.LearningService.Domain.Exceptions;

namespace learnova.LearningService.Domain.Entities
{
    public class Unit
    {
        private readonly List<Topic> _topics = new();

        public Guid Id { get; private set; }
        public Guid SubjectId { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<Topic> Topics => _topics.AsReadOnly();

        private Unit() { }

        public Unit(Guid subjectId, string title, string? description = null, int displayOrder = 0)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Unit title cannot be empty.");

            Id = Guid.NewGuid();
            SubjectId = subjectId;
            Title = title.Trim();
            Description = description?.Trim();
            DisplayOrder = displayOrder;
            IsActive = true;
        }

        // Persistence factory for reconstructing Unit
        internal static Unit Rehydrate(Guid id, Guid subjectId, string title, string? description, int displayOrder, bool isActive)
        {
            var u = new Unit()
            {
                Id = id,
                SubjectId = subjectId,
                Title = title,
                Description = description,
                DisplayOrder = displayOrder,
                IsActive = isActive
            };
            return u;
        }

        public void Update(string title, string? description, int displayOrder)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Unit title cannot be empty.");

            Title = title.Trim();
            Description = description?.Trim();
            DisplayOrder = displayOrder;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void AddTopic(Topic topic)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));
            if (_topics.Any(t => t.Id == topic.Id)) return;
            _topics.Add(topic);
        }
    }
}
