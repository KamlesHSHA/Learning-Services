using System;
using System.Collections.Generic;
using System.Linq;
using learnova.LearningService.Domain.Exceptions;

namespace learnova.LearningService.Domain.Entities
{
    public class Course
    {
        private readonly List<Subject> _subjects = new();

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public string Slug { get; private set; }
        public bool IsPublished { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public IReadOnlyCollection<Subject> Subjects => _subjects.AsReadOnly();

        private Course() { }

        private Course(Guid id, string title, string? description, string slug)
        {
            Id = id;
            Title = title;
            Description = description;
            Slug = slug;
            IsPublished = false;
            CreatedAt = DateTime.UtcNow;
        }

        public static Course Create(string title, string? description, string? slug = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Course title cannot be empty.");

            var id = Guid.NewGuid();
            var resolvedSlug = string.IsNullOrWhiteSpace(slug) ? GenerateSlug(title) : slug!;
            return new Course(id, title.Trim(), description?.Trim(), resolvedSlug);
        }

        public void UpdateMetadata(string title, string? description, string? slug = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new DomainException("Course title cannot be empty.");

            Title = title.Trim();
            Description = description?.Trim();
            if (!string.IsNullOrWhiteSpace(slug))
                Slug = slug!;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddSubject(Subject subject)
        {
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (_subjects.Any(s => s.Id == subject.Id)) return;
            _subjects.Add(subject);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveSubject(Guid subjectId)
        {
            var s = _subjects.FirstOrDefault(x => x.Id == subjectId);
            if (s != null)
            {
                _subjects.Remove(s);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Publish()
        {
            if (!HasMinimumStructure())
                throw new DomainException("Course cannot be published because it does not contain the minimum learning structure.");

            IsPublished = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            IsPublished = false;
            UpdatedAt = DateTime.UtcNow;
        }

        private bool HasMinimumStructure()
        {
            // Minimum: at least one subject with at least one unit with at least one topic.
            return _subjects.Any(s => s.Units != null && s.Units.Count > 0 && s.Units.Any(u => u.Topics != null && u.Topics.Count > 0));
        }

        private static string GenerateSlug(string title)
        {
            // Minimal slug generation: lowercase, replace spaces with '-', remove invalid chars.
            var slug = new string(title.ToLowerInvariant().Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-').ToArray());
            return string.Join('-', slug.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
