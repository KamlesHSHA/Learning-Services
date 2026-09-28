using System.Threading;
using System.Threading.Tasks;

namespace learnova.LearningService.Application.Commands.Courses
{
    public class CreateCourseCommand
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Slug { get; set; }
    }
}
