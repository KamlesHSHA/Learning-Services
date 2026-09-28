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
    public class UnitRepository : IUnitRepository
    {
        private readonly Container _container;

        public UnitRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Units);
        }

        public async Task AddAsync(Unit unit, CancellationToken cancellationToken = default)
        {
            var doc = new UnitDocument
            {
                id = unit.Id.ToString(),
                SubjectId = unit.SubjectId.ToString(),
                Title = unit.Title,
                Description = unit.Description,
                DisplayOrder = unit.DisplayOrder,
                IsActive = unit.IsActive
            };

            await _container.CreateItemAsync(doc, new PartitionKey(doc.SubjectId), cancellationToken: cancellationToken);
        }

        public async Task<Unit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
            var it = _container.GetItemQueryIterator<UnitDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
            while (it.HasMoreResults)
            {
                var r = await it.ReadNextAsync(cancellationToken);
                var doc = r.Resource.FirstOrDefault();
                if (doc != null) return MapToDomain(doc);
            }
            return null;
        }

        public async Task<IEnumerable<Unit>> GetBySubjectIdAsync(Guid subjectId, CancellationToken cancellationToken = default)
        {
            var pk = new PartitionKey(subjectId.ToString());
            var query = _container.GetItemQueryIterator<UnitDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
            var results = new List<UnitDocument>();
            while (query.HasMoreResults)
            {
                var r = await query.ReadNextAsync(cancellationToken);
                results.AddRange(r.Resource);
            }
            return results.Select(MapToDomain);
        }

        public async Task UpdateAsync(Unit unit, CancellationToken cancellationToken = default)
        {
            var doc = new UnitDocument
            {
                id = unit.Id.ToString(),
                SubjectId = unit.SubjectId.ToString(),
                Title = unit.Title,
                Description = unit.Description,
                DisplayOrder = unit.DisplayOrder,
                IsActive = unit.IsActive
            };

            await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.SubjectId), cancellationToken: cancellationToken);
        }

        private static Unit MapToDomain(UnitDocument d)
        {
            var unit = (Unit)Activator.CreateInstance(typeof(Unit), true)!;
            var t = typeof(Unit);
            t.GetProperty("Id")!.SetValue(unit, Guid.Parse(d.id));
            t.GetProperty("SubjectId")!.SetValue(unit, Guid.Parse(d.SubjectId));
            t.GetProperty("Title")!.SetValue(unit, d.Title);
            t.GetProperty("Description")!.SetValue(unit, d.Description);
            t.GetProperty("DisplayOrder")!.SetValue(unit, d.DisplayOrder);
            t.GetProperty("IsActive")!.SetValue(unit, d.IsActive);
            return unit;
        }
    }
}
