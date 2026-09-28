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
    public class TopicRepository : ITopicRepository
    {
        private readonly Container _container;
        private readonly Microsoft.Extensions.Logging.ILogger<TopicRepository> _logger;

        public TopicRepository(CosmosClient client, string databaseName, Microsoft.Extensions.Logging.ILogger<TopicRepository> logger)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Topics);
            _logger = logger;
        }

        public async Task AddAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            var doc = TopicMapper.MapToDocument(topic);
            try
            {
                _logger?.LogDebug("Creating topic {TopicId}", topic.Id);
                await _container.CreateItemAsync(doc, new PartitionKey(doc.UnitId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to create topic {TopicId}", topic.Id);
                throw new InvalidOperationException("Failed to create topic.", ex);
            }
        }

        public async Task<Topic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id, partitionKey: null, cancellationToken: cancellationToken);
        }

        public async Task<Topic?> GetByIdAsync(Guid id, string partitionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(partitionKey))
                {
                    // Use point read when partition key is known
                    try
                    {
                        var pk = new PartitionKey(partitionKey);
                        var resp = await _container.ReadItemAsync<TopicDocument>(id.ToString(), pk, cancellationToken: cancellationToken);
                        return TopicMapper.MapToDomain(resp.Resource);
                    }
                    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
                var it = _container.GetItemQueryIterator<TopicDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
                while (it.HasMoreResults)
                {
                    var r = await it.ReadNextAsync(cancellationToken);
                    var doc = r.Resource.FirstOrDefault();
                    if (doc != null) return TopicMapper.MapToDomain(doc);
                }
                return null;
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to get topic {TopicId}", id);
                throw new InvalidOperationException("Failed to get topic.", ex);
            }
        }

        public async Task<IEnumerable<Topic>> GetByUnitIdAsync(Guid unitId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pk = new PartitionKey(unitId.ToString());
                var query = _container.GetItemQueryIterator<TopicDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
                var results = new List<TopicDocument>();
                while (query.HasMoreResults)
                {
                    var r = await query.ReadNextAsync(cancellationToken);
                    results.AddRange(r.Resource);
                }
                return results.Select(TopicMapper.MapToDomain);
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to query topics for unit {UnitId}", unitId);
                throw new InvalidOperationException("Failed to query topics.", ex);
            }
        }

        public async Task UpdateAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            var doc = TopicMapper.MapToDocument(topic);
            try
            {
                _logger?.LogDebug("Updating topic {TopicId}", topic.Id);
                await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.UnitId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger?.LogError(ex, "Failed to update topic {TopicId}", topic.Id);
                throw new InvalidOperationException("Failed to update topic.", ex);
            }
        }
    }
}
