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
    public class ProgressRepository : IProgressRepository
    {
        private readonly Container _container;
        private readonly ILogger<ProgressRepository> _logger;

        public ProgressRepository(CosmosClient client, string databaseName, Microsoft.Extensions.Logging.ILogger<ProgressRepository> logger)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Progress);
            _logger = logger;
        }

        public async Task AddAsync(Progress progress, CancellationToken cancellationToken = default)
        {
            var doc = ProgressMapper.MapToDocument(progress);
            try
            {
                _logger.LogDebug("Creating progress {ProgressId} for user {UserId}", progress.Id, progress.UserId);
                await _container.CreateItemAsync(doc, new PartitionKey(doc.UserId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to create progress {ProgressId}", progress.Id);
                throw new InvalidOperationException("Failed to create progress.", ex);
            }
        }

        public async Task<Progress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id, userId: null, cancellationToken: cancellationToken);
        }

        public async Task<Progress?> GetByIdAsync(Guid id, string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    try
                    {
                        var pk = new PartitionKey(userId);
                        var resp = await _container.ReadItemAsync<ProgressDocument>(id.ToString(), pk, cancellationToken: cancellationToken);
                        return ProgressMapper.MapToDomain(resp.Resource);
                    }
                    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
                var it = _container.GetItemQueryIterator<ProgressDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
                while (it.HasMoreResults)
                {
                    var r = await it.ReadNextAsync(cancellationToken);
                    var doc = r.Resource.FirstOrDefault();
                    if (doc != null) return ProgressMapper.MapToDomain(doc);
                }
                return null;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to get progress {ProgressId}", id);
                throw new InvalidOperationException("Failed to get progress.", ex);
            }
        }

        public async Task<IEnumerable<Progress>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pk = new PartitionKey(userId);
                var query = _container.GetItemQueryIterator<ProgressDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
                var results = new List<ProgressDocument>();
                while (query.HasMoreResults)
                {
                    var r = await query.ReadNextAsync(cancellationToken);
                    results.AddRange(r.Resource);
                }
                return results.Select(ProgressMapper.MapToDomain);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to query progress for user {UserId}", userId);
                throw new InvalidOperationException("Failed to query progress.", ex);
            }
        }

        public async Task UpdateAsync(Progress progress, CancellationToken cancellationToken = default)
        {
            var doc = ProgressMapper.MapToDocument(progress);
            try
            {
                _logger.LogDebug("Updating progress {ProgressId} for user {UserId}", progress.Id, progress.UserId);
                await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.UserId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to update progress {ProgressId}", progress.Id);
                throw new InvalidOperationException("Failed to update progress.", ex);
            }
        }
    }
}
