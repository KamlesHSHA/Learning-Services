using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Courses;
using learnova.LearningService.Application.DTOs.Courses;
using learnova.LearningService.Application.Interfaces;

namespace learnova.LearningService.Application.Handlers.Courses
{
    public class UnpublishCourseHandler
    {
        private readonly ICourseRepository _courseRepository;

        public UnpublishCourseHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDto> HandleAsync(UnpublishCourseCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));

            var course = await _courseRepository.GetByIdAsync(command.Id, cancellationToken)
                         ?? throw new System.InvalidOperationException("Course not found.");

            course.Unpublish();

            await _courseRepository.UpdateAsync(course, cancellationToken);

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Slug = course.Slug,
                IsPublished = course.IsPublished,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            };
        }
    }
}
