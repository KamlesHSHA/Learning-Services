using System;
using FluentAssertions;
using learnova.LearningService.Persistence.Documents;
using learnova.LearningService.Persistence.Mappers;
using Xunit;

namespace learnova.LearningService.Persistence.Tests
{
    public class MappersRehydrationTests
    {
        [Fact]
        public void CourseMapper_Rehydrates_Correctly()
        {
            var doc = new CourseDocument
            {
                id = Guid.NewGuid().ToString(),
                Title = "Test Course",
                Description = "Desc",
                Slug = "test-course",
                IsPublished = true,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };

            var domain = CourseMapper.MapToDomain(doc);

            domain.Should().NotBeNull();
            domain.Id.Should().Be(Guid.Parse(doc.id));
            domain.Title.Should().Be(doc.Title);
            domain.Description.Should().Be(doc.Description);
            domain.Slug.Should().Be(doc.Slug);
        }

        [Fact]
        public void SubjectMapper_Rehydrates_Correctly()
        {
            var doc = new SubjectDocument
            {
                id = Guid.NewGuid().ToString(),
                CourseId = Guid.NewGuid().ToString(),
                Name = "Subject",
                Description = "SDesc",
                DisplayOrder = 2,
                IsActive = true
            };

            var domain = SubjectMapper.MapToDomain(doc);

            domain.Should().NotBeNull();
            domain.Id.Should().Be(Guid.Parse(doc.id));
            domain.CourseId.Should().Be(Guid.Parse(doc.CourseId));
            domain.Name.Should().Be(doc.Name);
        }

        [Fact]
        public void UnitMapper_Rehydrates_Correctly()
        {
            var doc = new UnitDocument
            {
                id = Guid.NewGuid().ToString(),
                SubjectId = Guid.NewGuid().ToString(),
                Title = "Unit",
                Description = "UDesc",
                DisplayOrder = 1,
                IsActive = true
            };

            var domain = UnitMapper.MapToDomain(doc);

            domain.Should().NotBeNull();
            domain.Id.Should().Be(Guid.Parse(doc.id));
            domain.SubjectId.Should().Be(Guid.Parse(doc.SubjectId));
            domain.Title.Should().Be(doc.Title);
        }

        [Fact]
        public void TopicMapper_Rehydrates_Correctly()
        {
            var doc = new TopicDocument
            {
                id = Guid.NewGuid().ToString(),
                UnitId = Guid.NewGuid().ToString(),
                Title = "Topic",
                Description = "TDesc",
                DisplayOrder = 3,
                IsActive = true
            };

            var domain = TopicMapper.MapToDomain(doc);

            domain.Should().NotBeNull();
            domain.Id.Should().Be(Guid.Parse(doc.id));
            domain.UnitId.Should().Be(Guid.Parse(doc.UnitId));
            domain.Title.Should().Be(doc.Title);
        }

        [Fact]
        public void LearningResourceMapper_Rehydrates_Correctly()
        {
            var doc = new LearningResourceDocument
            {
                id = Guid.NewGuid().ToString(),
                TopicId = Guid.NewGuid().ToString(),
                Title = "Res",
                Description = "RDesc",
                ResourceType = learnova.LearningService.Domain.Enums.ResourceType.Unknown,
                ResourceUri = "http://example",
                DisplayOrder = 0,
                IsPublished = false
            };

            var domain = LearningResourceMapper.MapToDomain(doc);

            domain.Should().NotBeNull();
            domain.Id.Should().Be(Guid.Parse(doc.id));
            domain.TopicId.Should().Be(Guid.Parse(doc.TopicId));
            domain.Title.Should().Be(doc.Title);
        }

        [Fact]
        public void ProgressMapper_Rehydrates_Correctly()
        {
            var doc = new ProgressDocument
            {
                id = Guid.NewGuid().ToString(),
                UserId = "user-1",
                CourseId = Guid.NewGuid().ToString(),
                SubjectId = Guid.NewGuid().ToString(),
                UnitId = Guid.NewGuid().ToString(),
                TopicId = Guid.NewGuid().ToString(),
                ResourceId = Guid.NewGuid().ToString(),
                CompletionPercentage = 45.5,
                IsCompleted = false,
                LastAccessedAt = DateTime.UtcNow.AddHours(-1),
                CompletedAt = null
            };

            var domain = ProgressMapper.MapToDomain(doc);

            domain.Should().NotBeNull();
            domain.Id.Should().Be(Guid.Parse(doc.id));
            domain.UserId.Should().Be(doc.UserId);
            domain.CompletionPercentage.Should().Be(doc.CompletionPercentage);
        }
    }
}
