using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Constants;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly Container _container;

        public CourseRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Courses);
        }

        public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
        {
            var doc = new CourseDocument
            {
                id = course.Id.ToString(),
                Title = course.Title,
                Description = course.Description,
                Slug = course.Slug,
                IsPublished = course.IsPublished,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };

            await _container.CreateItemAsync(doc, new PartitionKey(doc.id), cancellationToken: cancellationToken);
        }

        public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _container.ReadItemAsync<CourseDocument>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: cancellationToken);
                var d = response.Resource;
                return MapToDomain(d);
            }
            catch (CosmosException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var query = _container.GetItemQueryIterator<CourseDocument>("SELECT * FROM c");
            var results = new List<CourseDocument>();
            while (query.HasMoreResults)
            {
                var r = await query.ReadNextAsync(cancellationToken);
                results.AddRange(r.Resource);
            }

            return results.Select(MapToDomain);
        }

        public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
        {
            var doc = new CourseDocument
            {
                id = course.Id.ToString(),
                Title = course.Title,
                Description = course.Description,
                Slug = course.Slug,
                IsPublished = course.IsPublished,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };

            await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.id), cancellationToken: cancellationToken);
        }

        private static Course MapToDomain(CourseDocument d)
        {
            var course = (Course)Activator.CreateInstance(typeof(Course), true)!;
            // set private properties via reflection
            var t = typeof(Course);
            t.GetProperty("Id")!.SetValue(course, Guid.Parse(d.id));
            t.GetProperty("Title")!.SetValue(course, d.Title);
            t.GetProperty("Description")!.SetValue(course, d.Description);
            t.GetProperty("Slug")!.SetValue(course, d.Slug);
            t.GetProperty("IsPublished")!.SetValue(course, d.IsPublished);
            t.GetProperty("CreatedAt")!.SetValue(course, d.CreatedAt);
            t.GetProperty("UpdatedAt")!.SetValue(course, d.UpdatedAt);
            return course;
        }
    }
}
