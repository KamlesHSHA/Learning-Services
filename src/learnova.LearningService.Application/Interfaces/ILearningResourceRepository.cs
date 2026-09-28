using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Interfaces
{
    public interface ILearningResourceRepository
    {
        Task AddAsync(LearningResource resource, CancellationToken cancellationToken = default);
        Task<LearningResource?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        // Partition-key aware overload - when topicId is known, callers may use this to enable point reads.
        Task<LearningResource?> GetByIdAsync(Guid id, Guid topicId, CancellationToken cancellationToken = default);
        Task<IEnumerable<LearningResource>> GetByTopicIdAsync(Guid topicId, CancellationToken cancellationToken = default);
        Task UpdateAsync(LearningResource resource, CancellationToken cancellationToken = default);
    }
}
