using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using learnova.LearningService.Persistence.Mappers;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Domain.Entities;
using learnova.LearningService.Persistence.Constants;
using learnova.LearningService.Persistence.Documents;

namespace learnova.LearningService.Persistence.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly Container _container;
        private readonly Microsoft.Extensions.Logging.ILogger<CourseRepository> _logger;
        // noop: trigger file update

        public CourseRepository(CosmosClient client, string databaseName, Microsoft.Extensions.Logging.ILogger<CourseRepository> logger)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Courses);
            _logger = logger;
        }

        public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
        {
            var doc = CourseMapper.MapToDocument(course);

            try
            {
                _logger.LogDebug("Creating course {CourseId}", course.Id);
                await _container.CreateItemAsync(doc, new PartitionKey(doc.id), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to create course {CourseId}", course.Id);
                throw new InvalidOperationException("Failed to create course.", ex);
            }
        }

        public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id, partitionKey: null, cancellationToken: cancellationToken);
        }

        public async Task<Course?> GetByIdAsync(Guid id, string partitionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(partitionKey))
                {
                    try
                    {
                        var pk = new PartitionKey(partitionKey);
                        var resp = await _container.ReadItemAsync<CourseDocument>(id.ToString(), pk, cancellationToken: cancellationToken);
                        return CourseMapper.MapToDomain(resp.Resource);
                    }
                    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        _logger.LogInformation("Course {CourseId} not found.", id);
                        return null;
                    }
                }

                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
                var it = _container.GetItemQueryIterator<CourseDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
                while (it.HasMoreResults)
                {
                    var r = await it.ReadNextAsync(cancellationToken);
                    var doc = r.Resource.FirstOrDefault();
                    if (doc != null) return CourseMapper.MapToDomain(doc);
                }
                return null;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to read course {CourseId}", id);
                throw new InvalidOperationException("Failed to read course.", ex);
            }
        }

        public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var query = _container.GetItemQueryIterator<CourseDocument>("SELECT * FROM c");
            var results = new List<CourseDocument>();
            try
            {
                while (query.HasMoreResults)
                {
                    var r = await query.ReadNextAsync(cancellationToken);
                    results.AddRange(r.Resource);
                }
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to query courses.");
                throw new InvalidOperationException("Failed to query courses.", ex);
            }

            return results.Select(CourseMapper.MapToDomain);
        }

        public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
        {
            var doc = CourseMapper.MapToDocument(course);

            try
            {
                _logger.LogDebug("Updating course {CourseId}", course.Id);
                await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.id), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to update course {CourseId}", course.Id);
                throw new InvalidOperationException("Failed to update course.", ex);
            }
        }
    }
}
