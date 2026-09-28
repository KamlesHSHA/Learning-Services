using System;
using System.Collections.Generic;
using System.Linq;
using learnova.LearningService.Domain.Exceptions;

namespace learnova.LearningService.Domain.Entities
{
    public class Subject
    {
        private readonly List<Unit> _units = new();

        public Guid Id { get; private set; }
        public Guid CourseId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public int DisplayOrder { get; private set; }
        public bool IsActive { get; private set; }

        public IReadOnlyCollection<Unit> Units => _units.AsReadOnly();

        private Subject() { }

        public Subject(Guid courseId, string name, string? description = null, int displayOrder = 0)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Subject name cannot be empty.");

            Id = Guid.NewGuid();
            CourseId = courseId;
            Name = name.Trim();
            Description = description?.Trim();
            DisplayOrder = displayOrder;
            IsActive = true;
        }

        // Persistence factory for reconstructing Subject
        internal static Subject Rehydrate(Guid id, Guid courseId, string name, string? description, int displayOrder, bool isActive)
        {
            var s = new Subject()
            {
                Id = id,
                CourseId = courseId,
                Name = name,
                Description = description,
                DisplayOrder = displayOrder,
                IsActive = isActive
            };
            return s;
        }

        public void Update(string name, string? description, int displayOrder)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Subject name cannot be empty.");

            Name = name.Trim();
            Description = description?.Trim();
            DisplayOrder = displayOrder;
        }

        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;

        public void AddUnit(Unit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            if (_units.Any(u => u.Id == unit.Id)) return;
            _units.Add(unit);
        }
    }
}
