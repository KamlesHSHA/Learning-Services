using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Units;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Units;

namespace learnova.LearningService.Application.Handlers.Units
{
    public class GetUnitByIdHandler
    {
        private readonly IUnitRepository _unitRepository;

        public GetUnitByIdHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<UnitDto?> HandleAsync(GetUnitByIdQuery query, CancellationToken cancellationToken = default)
        {
            var unit = await _unitRepository.GetByIdAsync(query.Id, cancellationToken);
            if (unit == null) return null;

            return new UnitDto
            {
                Id = unit.Id,
                SubjectId = unit.SubjectId,
                Title = unit.Title,
                Description = unit.Description,
                DisplayOrder = unit.DisplayOrder,
                IsActive = unit.IsActive
            };
        }
    }
}
