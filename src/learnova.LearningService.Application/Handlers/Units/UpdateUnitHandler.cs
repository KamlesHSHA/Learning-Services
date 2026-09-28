using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Units;
using learnova.LearningService.Application.DTOs.Units;
using learnova.LearningService.Application.Interfaces;

namespace learnova.LearningService.Application.Handlers.Units
{
    public class UpdateUnitHandler
    {
        private readonly IUnitRepository _unitRepository;

        public UpdateUnitHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<UnitDto> HandleAsync(UpdateUnitCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));

            var unit = await _unitRepository.GetByIdAsync(command.Id, cancellationToken)
                       ?? throw new System.InvalidOperationException("Unit not found.");

            unit.Update(command.Title, command.Description, command.DisplayOrder);

            await _unitRepository.UpdateAsync(unit, cancellationToken);

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
