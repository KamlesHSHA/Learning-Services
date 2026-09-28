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
    public class TopicRepository : ITopicRepository
    {
        private readonly Container _container;

        public TopicRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Topics);
        }

        public async Task AddAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            var doc = new TopicDocument
            {
                id = topic.Id.ToString(),
                UnitId = topic.UnitId.ToString(),
                Title = topic.Title,
                Description = topic.Description,
                DisplayOrder = topic.DisplayOrder,
                IsActive = topic.IsActive
            };

            await _container.CreateItemAsync(doc, new PartitionKey(doc.UnitId), cancellationToken: cancellationToken);
        }

        public async Task<Topic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
            var it = _container.GetItemQueryIterator<TopicDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
            while (it.HasMoreResults)
            {
                var r = await it.ReadNextAsync(cancellationToken);
                var doc = r.Resource.FirstOrDefault();
                if (doc != null) return MapToDomain(doc);
            }
            return null;
        }

        public async Task<IEnumerable<Topic>> GetByUnitIdAsync(Guid unitId, CancellationToken cancellationToken = default)
        {
            var pk = new PartitionKey(unitId.ToString());
            var query = _container.GetItemQueryIterator<TopicDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
            var results = new List<TopicDocument>();
            while (query.HasMoreResults)
            {
                var r = await query.ReadNextAsync(cancellationToken);
                results.AddRange(r.Resource);
            }
            return results.Select(MapToDomain);
        }

        public async Task UpdateAsync(Topic topic, CancellationToken cancellationToken = default)
        {
            var doc = new TopicDocument
            {
                id = topic.Id.ToString(),
                UnitId = topic.UnitId.ToString(),
                Title = topic.Title,
                Description = topic.Description,
                DisplayOrder = topic.DisplayOrder,
                IsActive = topic.IsActive
            };

            await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.UnitId), cancellationToken: cancellationToken);
        }

        private static Topic MapToDomain(TopicDocument d)
        {
            var topic = (Topic)Activator.CreateInstance(typeof(Topic), true)!;
            var t = typeof(Topic);
            t.GetProperty("Id")!.SetValue(topic, Guid.Parse(d.id));
            t.GetProperty("UnitId")!.SetValue(topic, Guid.Parse(d.UnitId));
            t.GetProperty("Title")!.SetValue(topic, d.Title);
            t.GetProperty("Description")!.SetValue(topic, d.Description);
            t.GetProperty("DisplayOrder")!.SetValue(topic, d.DisplayOrder);
            t.GetProperty("IsActive")!.SetValue(topic, d.IsActive);
            return topic;
        }
    }
}
