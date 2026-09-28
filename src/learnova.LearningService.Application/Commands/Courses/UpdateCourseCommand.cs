using System;

namespace learnova.LearningService.Application.Commands.Courses
{
    public class UpdateCourseCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Slug { get; set; }
    }
}
