using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Subjects;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Subjects;

namespace learnova.LearningService.Application.Handlers.Subjects
{
    public class GetSubjectByIdHandler
    {
        private readonly ISubjectRepository _subjectRepository;

        public GetSubjectByIdHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<SubjectDto?> HandleAsync(GetSubjectByIdQuery query, CancellationToken cancellationToken = default)
        {
            var subject = await _subjectRepository.GetByIdAsync(query.Id, cancellationToken);
            if (subject == null) return null;

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
