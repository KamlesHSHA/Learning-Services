using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Interfaces
{
    public interface IUnitRepository
    {
        Task AddAsync(Unit unit, CancellationToken cancellationToken = default);
        Task<Unit?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Unit>> GetBySubjectIdAsync(Guid subjectId, CancellationToken cancellationToken = default);
        Task UpdateAsync(Unit unit, CancellationToken cancellationToken = default);
    }
}
