using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Interfaces
{
    public interface IProgressRepository
    {
        Task AddAsync(Progress progress, CancellationToken cancellationToken = default);
        Task<Progress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Progress>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task UpdateAsync(Progress progress, CancellationToken cancellationToken = default);
    }
}
