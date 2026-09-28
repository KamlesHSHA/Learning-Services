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
        // Partition-key aware overload — when a partition key is available (not used for Courses currently), callers can use this signature. Keep for parity.
        Task<Course?> GetByIdAsync(Guid id, string partitionKey, CancellationToken cancellationToken = default);
        Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default);
        Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
    }
}

