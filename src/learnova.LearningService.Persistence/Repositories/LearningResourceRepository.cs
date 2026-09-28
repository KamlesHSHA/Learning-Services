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
    public class LearningResourceRepository : ILearningResourceRepository
    {
        private readonly Container _container;
        private readonly Microsoft.Extensions.Logging.ILogger<LearningResourceRepository> _logger;

        public LearningResourceRepository(CosmosClient client, string databaseName, Microsoft.Extensions.Logging.ILogger<LearningResourceRepository> logger)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.LearningResources);
            _logger = logger;
        }

        public async Task AddAsync(LearningResource resource, CancellationToken cancellationToken = default)
        {
            var doc = LearningResourceMapper.MapToDocument(resource);
            try
            {
                _logger.LogDebug("Creating learning resource {ResourceId}", resource.Id);
                await _container.CreateItemAsync(doc, new PartitionKey(doc.TopicId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to create learning resource {ResourceId}", resource.Id);
                throw new InvalidOperationException("Failed to create learning resource.", ex);
            }
        }

        public async Task<LearningResource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
                var it = _container.GetItemQueryIterator<LearningResourceDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
                while (it.HasMoreResults)
                {
                    var r = await it.ReadNextAsync(cancellationToken);
                    var doc = r.Resource.FirstOrDefault();
                    if (doc != null) return LearningResourceMapper.MapToDomain(doc);
                }
                return null;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to get learning resource {ResourceId}", id);
                throw new InvalidOperationException("Failed to get learning resource.", ex);
            }
        }

        public async Task<IEnumerable<LearningResource>> GetByTopicIdAsync(Guid topicId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pk = new PartitionKey(topicId.ToString());
                var query = _container.GetItemQueryIterator<LearningResourceDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
                var results = new List<LearningResourceDocument>();
                while (query.HasMoreResults)
                {
                    var r = await query.ReadNextAsync(cancellationToken);
                    results.AddRange(r.Resource);
                }
                return results.Select(LearningResourceMapper.MapToDomain);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to query resources for topic {TopicId}", topicId);
                throw new InvalidOperationException("Failed to query resources.", ex);
            }
        }

        public async Task UpdateAsync(LearningResource resource, CancellationToken cancellationToken = default)
        {
            var doc = LearningResourceMapper.MapToDocument(resource);
            try
            {
                _logger.LogDebug("Updating learning resource {ResourceId}", resource.Id);
                await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.TopicId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to update learning resource {ResourceId}", resource.Id);
                throw new InvalidOperationException("Failed to update learning resource.", ex);
            }
        }
    }
}
