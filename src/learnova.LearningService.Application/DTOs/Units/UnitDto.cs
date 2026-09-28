using System;

namespace learnova.LearningService.Application.DTOs.Units
{
    public class UnitDto
    {
        public Guid Id { get; set; }
        public Guid SubjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
