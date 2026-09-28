using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Interfaces
{
    // Repository abstractions belong in the Application layer. Keep them minimal for the foundation.
    public interface ICourseRepository
    {
        Task AddAsync(Course course, CancellationToken cancellationToken = default);
        Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    }
}

