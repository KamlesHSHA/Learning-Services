using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Subjects;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Subjects;

namespace learnova.LearningService.Application.Handlers.Subjects
{
    public class GetSubjectsByCourseHandler
    {
        private readonly ISubjectRepository _subjectRepository;

        public GetSubjectsByCourseHandler(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        public async Task<IEnumerable<SubjectDto>> HandleAsync(GetSubjectsByCourseQuery query, CancellationToken cancellationToken = default)
        {
            var items = await _subjectRepository.GetByCourseIdAsync(query.CourseId, cancellationToken);
            return items.Select(s => new SubjectDto
            {
                Id = s.Id,
                CourseId = s.CourseId,
                Name = s.Name,
                Description = s.Description,
                DisplayOrder = s.DisplayOrder,
                IsActive = s.IsActive
            });
        }
    }
}
