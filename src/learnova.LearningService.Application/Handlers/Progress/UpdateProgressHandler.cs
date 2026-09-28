using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Progress;
using learnova.LearningService.Application.DTOs.Progress;
using learnova.LearningService.Application.Interfaces;

namespace learnova.LearningService.Application.Handlers.Progress
{
    public class UpdateProgressHandler
    {
        private readonly IProgressRepository _progressRepository;

        public UpdateProgressHandler(IProgressRepository progressRepository)
        {
            _progressRepository = progressRepository;
        }

        public async Task<ProgressDto> HandleAsync(UpdateProgressCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));
            if (command.CompletionPercentage < 0 || command.CompletionPercentage > 100)
                throw new System.ArgumentOutOfRangeException(nameof(command.CompletionPercentage), "Completion must be between 0 and 100.");

            var progress = await _progressRepository.GetByIdAsync(command.ProgressId, cancellationToken)
                           ?? throw new System.InvalidOperationException("Progress not found.");

            progress.UpdateCompletion(command.CompletionPercentage);
            await _progressRepository.UpdateAsync(progress, cancellationToken);

            return new ProgressDto
            {
                Id = progress.Id,
                UserId = progress.UserId,
                CourseId = progress.CourseId,
                SubjectId = progress.SubjectId,
                UnitId = progress.UnitId,
                TopicId = progress.TopicId,
                ResourceId = progress.ResourceId,
                CompletionPercentage = progress.CompletionPercentage,
                IsCompleted = progress.IsCompleted,
                LastAccessedAt = progress.LastAccessedAt,
                CompletedAt = progress.CompletedAt
            };
        }
    }
}
