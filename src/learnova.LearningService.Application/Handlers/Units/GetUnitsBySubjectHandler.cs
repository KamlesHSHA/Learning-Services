using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Units;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Units;

namespace learnova.LearningService.Application.Handlers.Units
{
    public class GetUnitsBySubjectHandler
    {
        private readonly IUnitRepository _unitRepository;

        public GetUnitsBySubjectHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<IEnumerable<UnitDto>> HandleAsync(GetUnitsBySubjectQuery query, CancellationToken cancellationToken = default)
        {
            var items = await _unitRepository.GetBySubjectIdAsync(query.SubjectId, cancellationToken);
            return items.Select(u => new UnitDto
            {
                Id = u.Id,
                SubjectId = u.SubjectId,
                Title = u.Title,
                Description = u.Description,
                DisplayOrder = u.DisplayOrder,
                IsActive = u.IsActive
            });
        }
    }
}
