using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Subjects;
using learnova.LearningService.Application.DTOs.Subjects;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Handlers.Subjects
{
    public class CreateSubjectHandler
    {
        private readonly ISubjectRepository _subjectRepository;

        public CreateSubjectHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<SubjectDto> HandleAsync(CreateSubjectCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));
            if (string.IsNullOrWhiteSpace(command.Name)) throw new System.ArgumentException("Name is required.", nameof(command.Name));

            var subject = new Subject(command.CourseId, command.Name, command.Description, command.DisplayOrder);

            await _subjectRepository.AddAsync(subject, cancellationToken);

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
