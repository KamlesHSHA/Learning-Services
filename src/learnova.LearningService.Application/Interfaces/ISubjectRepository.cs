using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Interfaces
{
    public interface ISubjectRepository
    {
        Task AddAsync(Subject subject, CancellationToken cancellationToken = default);
        Task<Subject?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Subject>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
        Task UpdateAsync(Subject subject, CancellationToken cancellationToken = default);
    }
}
