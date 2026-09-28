using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Units;
using learnova.LearningService.Application.DTOs.Units;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Handlers.Units
{
    public class CreateUnitHandler
    {
        private readonly IUnitRepository _unitRepository;

        public CreateUnitHandler(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public async Task<UnitDto> HandleAsync(CreateUnitCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));
            if (string.IsNullOrWhiteSpace(command.Title)) throw new System.ArgumentException("Title is required.", nameof(command.Title));

            var unit = new Unit(command.SubjectId, command.Title, command.Description, command.DisplayOrder);

            await _unitRepository.AddAsync(unit, cancellationToken);

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
