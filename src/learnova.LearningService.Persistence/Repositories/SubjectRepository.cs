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
    public class SubjectRepository : ISubjectRepository
    {
        private readonly Container _container;

        public SubjectRepository(CosmosClient client, string databaseName)
        {
            _container = client.GetContainer(databaseName, CosmosContainerNames.Subjects);
        }

        public async Task AddAsync(Subject subject, CancellationToken cancellationToken = default)
        {
            var doc = new SubjectDocument
            {
                id = subject.Id.ToString(),
                CourseId = subject.CourseId.ToString(),
                Name = subject.Name,
                Description = subject.Description,
                DisplayOrder = subject.DisplayOrder,
                IsActive = subject.IsActive
            };

            await _container.CreateItemAsync(doc, new PartitionKey(doc.CourseId), cancellationToken: cancellationToken);
        }

        public async Task<Subject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var query = new QueryDefinition("SELECT * FROM c WHERE c.id = @id").WithParameter("@id", id.ToString());
            var it = _container.GetItemQueryIterator<SubjectDocument>(query, requestOptions: new QueryRequestOptions { MaxItemCount = 1 });
            while (it.HasMoreResults)
            {
                var r = await it.ReadNextAsync(cancellationToken);
                var doc = r.Resource.FirstOrDefault();
                if (doc != null) return MapToDomain(doc);
            }
            return null;
        }

        public async Task<IEnumerable<Subject>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
        {
            var pk = new PartitionKey(courseId.ToString());
            var query = _container.GetItemQueryIterator<SubjectDocument>(new QueryDefinition("SELECT * FROM c"), requestOptions: new QueryRequestOptions { PartitionKey = pk });
            var results = new List<SubjectDocument>();
            while (query.HasMoreResults)
            {
                var r = await query.ReadNextAsync(cancellationToken);
                results.AddRange(r.Resource);
            }
            return results.Select(MapToDomain);
        }

        public async Task UpdateAsync(Subject subject, CancellationToken cancellationToken = default)
        {
            var doc = new SubjectDocument
            {
                id = subject.Id.ToString(),
                CourseId = subject.CourseId.ToString(),
                Name = subject.Name,
                Description = subject.Description,
                DisplayOrder = subject.DisplayOrder,
                IsActive = subject.IsActive
            };

            await _container.ReplaceItemAsync(doc, doc.id, new PartitionKey(doc.CourseId), cancellationToken: cancellationToken);
        }

        private static Subject MapToDomain(SubjectDocument d)
        {
            var subj = (Subject)Activator.CreateInstance(typeof(Subject), true)!;
            var t = typeof(Subject);
            t.GetProperty("Id")!.SetValue(subj, Guid.Parse(d.id));
            t.GetProperty("CourseId")!.SetValue(subj, Guid.Parse(d.CourseId));
            t.GetProperty("Name")!.SetValue(subj, d.Name);
            t.GetProperty("Description")!.SetValue(subj, d.Description);
            t.GetProperty("DisplayOrder")!.SetValue(subj, d.DisplayOrder);
            t.GetProperty("IsActive")!.SetValue(subj, d.IsActive);
            return subj;
        }
    }
}
