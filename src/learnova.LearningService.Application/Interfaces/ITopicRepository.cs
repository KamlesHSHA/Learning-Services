using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Interfaces
{
    public interface ITopicRepository
    {
        Task AddAsync(Topic topic, CancellationToken cancellationToken = default);
        Task<Topic?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Topic>> GetByUnitIdAsync(Guid unitId, CancellationToken cancellationToken = default);
        Task UpdateAsync(Topic topic, CancellationToken cancellationToken = default);
    }
}
