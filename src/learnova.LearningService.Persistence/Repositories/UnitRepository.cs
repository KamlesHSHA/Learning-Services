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
    public class UnitRepository : IUnitRepository
    {
        private readonly Container _container;
        private readonly Microsoft.Extensions.Logging.ILogger<UnitRepository> _logger;

        public UnitRepository(CosmosClient client, string databaseName, Microsoft.Extensions.Logging.ILogger<UnitRepository> logger)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Units);
            _logger = logger;
        }

        public async Task AddAsync(Unit unit, CancellationToken cancellationToken = default)
        {
            var doc = UnitMapper.MapToDocument(unit);
            try
            {
                _logger.LogDebug("Creating unit {UnitId}", unit.Id);
                await _container.CreateItemAsync(doc, new PartitionKey(doc.SubjectId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to create unit {UnitId}", unit.Id);
                throw new InvalidOperationException("Failed to create unit.", ex);
            }
        }

        public async Task<Unit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id, partitionKey: null, cancellationToken: cancellationToken);
        }

        public async Task<Unit?> GetByIdAsync(Guid id, Guid subjectId, CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(id, partitionKey: subjectId.ToString(), cancellationToken: cancellationToken);
        }

        private async Task<Unit?> GetByIdAsync(Guid id, string? partitionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!string.IsNullOrEmpty(partitionKey))
                {
                    try
                    {
                        var pk = new PartitionKey(partitionKey);
                        var resp = await _container.ReadItemAsync<UnitDocument>(id.ToString(), pk, cancellationToken: cancellationToken);
                        return UnitMapper.MapToDomain(resp.Resource);
                    }
                    catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                }

                var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
                var it = _container.GetItemQueryIterator<UnitDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
                while (it.HasMoreResults)
                {
                    var r = await it.ReadNextAsync(cancellationToken);
                    var doc = r.Resource.FirstOrDefault();
                    if (doc != null) return UnitMapper.MapToDomain(doc);
                }
                return null;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to get unit {UnitId}", id);
                throw new InvalidOperationException("Failed to get unit.", ex);
            }
        }

        public async Task<IEnumerable<Unit>> GetBySubjectIdAsync(Guid subjectId, CancellationToken cancellationToken = default)
        {
            try
            {
                var pk = new PartitionKey(subjectId.ToString());
                var query = _container.GetItemQueryIterator<UnitDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
                var results = new List<UnitDocument>();
                while (query.HasMoreResults)
                {
                    var r = await query.ReadNextAsync(cancellationToken);
                    results.AddRange(r.Resource);
                }
                return results.Select(UnitMapper.MapToDomain);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to query units for subject {SubjectId}", subjectId);
                throw new InvalidOperationException("Failed to query units.", ex);
            }
        }

        public async Task UpdateAsync(Unit unit, CancellationToken cancellationToken = default)
        {
            var doc = UnitMapper.MapToDocument(unit);
            try
            {
                _logger.LogDebug("Updating unit {UnitId}", unit.Id);
                await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.SubjectId), cancellationToken: cancellationToken);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to update unit {UnitId}", unit.Id);
                throw new InvalidOperationException("Failed to update unit.", ex);
            }
        }
    }
}
