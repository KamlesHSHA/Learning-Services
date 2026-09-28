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
    public class SubjectRepository : ISubjectRepository
    {
        private readonly Container _container;
        private readonly Microsoft.Extensions.Logging.ILogger<SubjectRepository> _logger;

        public SubjectRepository(CosmosClient client, string databaseName, Microsoft.Extensions.Logging.ILogger<SubjectRepository> logger)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Subjects);
            _logger = logger;
        }

        public async Task AddAsync(Subject subject, CancellationToken cancellationToken = default)
        {
            var doc = SubjectMapper.MapToDocument(subject);

            try
            {
                _logger?.LogDebug("Creating subject {SubjectId}", subject.Id);
                await _container.CreateItemAsync(doc, new PartitionKey(doc.CourseId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to create subject {SubjectId}", subject.Id);
                throw new InvalidOperationException("Failed to create subject.", ex);
            }
        }

        public async Task<Subject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id, courseId: null, cancellationToken: cancellationToken);
        }

        public async Task<Subject?> GetByIdAsync(Guid id, Guid? courseId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (courseId.HasValue)
                {
                    try
                    {
                        var pk = new PartitionKey(courseId.Value.ToString());
                        var resp = await _container.ReadItemAsync<SubjectDocument>(id.ToString(), pk, cancellationToken: cancellationToken);
                        return SubjectMapper.MapToDomain(resp.Resource);
                    }
                    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
                var it = _container.GetItemQueryIterator<SubjectDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
                while (it.HasMoreResults)
                {
                    var r = await it.ReadNextAsync(cancellationToken);
                    var doc = r.Resource.FirstOrDefault();
                    if (doc != null) return SubjectMapper.MapToDomain(doc);
                }
                return null;
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to get subject {SubjectId}", id);
                throw new InvalidOperationException("Failed to get subject.", ex);
            }
        }

        public async Task<IEnumerable<Subject>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pk = new PartitionKey(courseId.ToString());
                var query = _container.GetItemQueryIterator<SubjectDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
                var results = new List<SubjectDocument>();
                while (query.HasMoreResults)
                {
                    var r = await query.ReadNextAsync(cancellationToken);
                    results.AddRange(r.Resource);
                }
                return results.Select(SubjectMapper.MapToDomain);
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to query subjects for course {CourseId}", courseId);
                throw new InvalidOperationException("Failed to query subjects.", ex);
            }
        }

        public async Task UpdateAsync(Subject subject, CancellationToken cancellationToken = default)
        {
            var doc = SubjectMapper.MapToDocument(subject);

            try
            {
                _logger?.LogDebug("Updating subject {SubjectId}", subject.Id);
                await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.CourseId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to update subject {SubjectId}", subject.Id);
                throw new InvalidOperationException("Failed to update subject.", ex);
            }
        }
    }
}
