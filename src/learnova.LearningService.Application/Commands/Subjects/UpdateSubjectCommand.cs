using System;

namespace learnova.LearningService.Application.Commands.Subjects
{
    public class UpdateSubjectCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}
