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

        public LearningResourceRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.LearningResources);
        }

        public async Task AddAsync(LearningResource resource, CancellationToken cancellationToken = default)
        {
            var doc = new LearningResourceDocument
            {
                id = resource.Id.ToString(),
                TopicId = resource.TopicId.ToString(),
                Title = resource.Title,
                Description = resource.Description,
                ResourceType = resource.ResourceType,
                ResourceUri = resource.ResourceUri,
                DisplayOrder = resource.DisplayOrder,
                IsPublished = resource.IsPublished
            };

            await _container.CreateItemAsync(doc, new PartitionKey(doc.TopicId), cancellationToken: cancellationToken);
        }

        public async Task<LearningResource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
            var it = _container.GetItemQueryIterator<LearningResourceDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
            while (it.HasMoreResults)
            {
                var r = await it.ReadNextAsync(cancellationToken);
                var doc = r.Resource.FirstOrDefault();
                if (doc != null) return MapToDomain(doc);
            }
            return null;
        }

        public async Task<IEnumerable<LearningResource>> GetByTopicIdAsync(Guid topicId, CancellationToken cancellationToken = default)
        {
            var pk = new PartitionKey(topicId.ToString());
            var query = _container.GetItemQueryIterator<LearningResourceDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
            var results = new List<LearningResourceDocument>();
            while (query.HasMoreResults)
            {
                var r = await query.ReadNextAsync(cancellationToken);
                results.AddRange(r.Resource);
            }
            return results.Select(MapToDomain);
        }

        public async Task UpdateAsync(LearningResource resource, CancellationToken cancellationToken = default)
        {
            var doc = new LearningResourceDocument
            {
                id = resource.Id.ToString(),
                TopicId = resource.TopicId.ToString(),
                Title = resource.Title,
                Description = resource.Description,
                ResourceType = resource.ResourceType,
                ResourceUri = resource.ResourceUri,
                DisplayOrder = resource.DisplayOrder,
                IsPublished = resource.IsPublished
            };

            await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.TopicId), cancellationToken: cancellationToken);
        }

        private static LearningResource MapToDomain(LearningResourceDocument d)
        {
            var resource = (LearningResource)Activator.CreateInstance(typeof(LearningResource), true)!;
            var t = typeof(LearningResource);
            t.GetProperty("Id")!.SetValue(resource, Guid.Parse(d.id));
            t.GetProperty("TopicId")!.SetValue(resource, Guid.Parse(d.TopicId));
            t.GetProperty("Title")!.SetValue(resource, d.Title);
            t.GetProperty("Description")!.SetValue(resource, d.Description);
            t.GetProperty("ResourceType")!.SetValue(resource, d.ResourceType);
            t.GetProperty("ResourceUri")!.SetValue(resource, d.ResourceUri);
            t.GetProperty("DisplayOrder")!.SetValue(resource, d.DisplayOrder);
            t.GetProperty("IsPublished")!.SetValue(resource, d.IsPublished);
            return resource;
        }
    }
}
