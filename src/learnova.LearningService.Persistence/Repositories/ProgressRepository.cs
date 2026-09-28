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
    public class ProgressRepository : IProgressRepository
    {
        private readonly Container _container;

        public ProgressRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Progress);
        }

        public async Task AddAsync(Progress progress, CancellationToken cancellationToken = default)
        {
            var doc = new ProgressDocument
            {
                id = progress.Id.ToString(),
                UserId = progress.UserId,
                CourseId = progress.CourseId?.ToString(),
                SubjectId = progress.SubjectId?.ToString(),
                UnitId = progress.UnitId?.ToString(),
                TopicId = progress.TopicId?.ToString(),
                ResourceId = progress.ResourceId?.ToString(),
                CompletionPercentage = progress.CompletionPercentage,
                IsCompleted = progress.IsCompleted,
                LastAccessedAt = progress.LastAccessedAt,
                CompletedAt = progress.CompletedAt
            };

            await _container.CreateItemAsync(doc, new PartitionKey(doc.UserId), cancellationToken: cancellationToken);
        }

        public async Task<Progress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
            var it = _container.GetItemQueryIterator<ProgressDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
            while (it.HasMoreResults)
            {
                var r = await it.ReadNextAsync(cancellationToken);
                var doc = r.Resource.FirstOrDefault();
                if (doc != null) return MapToDomain(doc);
            }
            return null;
        }

        public async Task<IEnumerable<Progress>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            var pk = new PartitionKey(userId);
            var query = _container.GetItemQueryIterator<ProgressDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
            var results = new List<ProgressDocument>();
            while (query.HasMoreResults)
            {
                var r = await query.ReadNextAsync(cancellationToken);
                results.AddRange(r.Resource);
            }
            return results.Select(MapToDomain);
        }

        public async Task UpdateAsync(Progress progress, CancellationToken cancellationToken = default)
        {
            var doc = new ProgressDocument
            {
                id = progress.Id.ToString(),
                UserId = progress.UserId,
                CourseId = progress.CourseId?.ToString(),
                SubjectId = progress.SubjectId?.ToString(),
                UnitId = progress.UnitId?.ToString(),
                TopicId = progress.TopicId?.ToString(),
                ResourceId = progress.ResourceId?.ToString(),
                CompletionPercentage = progress.CompletionPercentage,
                IsCompleted = progress.IsCompleted,
                LastAccessedAt = progress.LastAccessedAt,
                CompletedAt = progress.CompletedAt
            };

            await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.UserId), cancellationToken: cancellationToken);
        }

        private static Progress MapToDomain(ProgressDocument d)
        {
            var progress = (Progress)Activator.CreateInstance(typeof(Progress), true)!;
            var t = typeof(Progress);
            t.GetProperty("Id")!.SetValue(progress, Guid.Parse(d.id));
            t.GetProperty("UserId")!.SetValue(progress, d.UserId);
            t.GetProperty("CourseId")!.SetValue(progress, string.IsNullOrEmpty(d.CourseId) ? null : Guid.Parse(d.CourseId));
            t.GetProperty("SubjectId")!.SetValue(progress, string.IsNullOrEmpty(d.SubjectId) ? null : Guid.Parse(d.SubjectId));
            t.GetProperty("UnitId")!.SetValue(progress, string.IsNullOrEmpty(d.UnitId) ? null : Guid.Parse(d.UnitId));
            t.GetProperty("TopicId")!.SetValue(progress, string.IsNullOrEmpty(d.TopicId) ? null : Guid.Parse(d.TopicId));
            t.GetProperty("ResourceId")!.SetValue(progress, string.IsNullOrEmpty(d.ResourceId) ? null : Guid.Parse(d.ResourceId));
            t.GetProperty("CompletionPercentage")!.SetValue(progress, d.CompletionPercentage);
            t.GetProperty("IsCompleted")!.SetValue(progress, d.IsCompleted);
            t.GetProperty("LastAccessedAt")!.SetValue(progress, d.LastAccessedAt);
            t.GetProperty("CompletedAt")!.SetValue(progress, d.CompletedAt);
            return progress;
        }
    }
}
