using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Subjects;
using learnova.LearningService.Application.DTOs.Subjects;
using learnova.LearningService.Application.Interfaces;

namespace learnova.LearningService.Application.Handlers.Subjects
{
    public class UpdateSubjectHandler
    {
        private readonly ISubjectRepository _subjectRepository;

        public UpdateSubjectHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<SubjectDto> HandleAsync(UpdateSubjectCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));

            var subject = await _subjectRepository.GetByIdAsync(command.Id, cancellationToken)
                          ?? throw new System.InvalidOperationException("Subject not found.");

            subject.Update(command.Name, command.Description, command.DisplayOrder);

            await _subjectRepository.UpdateAsync(subject, cancellationToken);

            return new SubjectDto
            {
                Id = subject.Id,
                CourseId = subject.CourseId,
                Name = subject.Name,
                Description = subject.Description,
                DisplayOrder = subject.DisplayOrder,
                IsActive = subject.IsActive
            };
        }
    }
}
