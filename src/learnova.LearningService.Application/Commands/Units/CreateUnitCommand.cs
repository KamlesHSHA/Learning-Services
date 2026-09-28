using System;

namespace learnova.LearningService.Application.Commands.Units
{
    public class CreateUnitCommand
    {
        public Guid SubjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
