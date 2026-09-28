using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Progress;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Progress;

namespace learnova.LearningService.Application.Handlers.Progress
{
    public class GetProgressByUserHandler
    {
        private readonly IProgressRepository _progressRepository;

        public GetProgressByUserHandler(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        public async Task<IEnumerable<ProgressDto>> HandleAsync(GetProgressByUserQuery query, CancellationToken cancellationToken = default)
        {
            var items = await _progressRepository.GetByUserIdAsync(query.UserId, cancellationToken);

            return items.Select(p => new ProgressDto
            {
                Id = p.Id,
                UserId = p.UserId,
                CourseId = p.CourseId,
                SubjectId = p.SubjectId,
                UnitId = p.UnitId,
                TopicId = p.TopicId,
                ResourceId = p.ResourceId,
                CompletionPercentage = p.CompletionPercentage,
                IsCompleted = p.IsCompleted,
                LastAccessedAt = p.LastAccessedAt,
                CompletedAt = p.CompletedAt
            });
        }
    }
}
