using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.Commands.Courses;
using learnova.LearningService.Application.DTOs.Courses;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Domain.Entities;

namespace learnova.LearningService.Application.Handlers.Courses
{
    public class CreateCourseHandler
    {
        private readonly ICourseRepository _courseRepository;

        public CreateCourseHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDto> HandleAsync(CreateCourseCommand command, CancellationToken cancellationToken = default)
        {
            if (command == null) throw new System.ArgumentNullException(nameof(command));
            if (string.IsNullOrWhiteSpace(command.Title)) throw new System.ArgumentException("Title is required", nameof(command.Title));

            var course = Course.Create(command.Title, command.Description, command.Slug);

            await _courseRepository.AddAsync(course, cancellationToken);

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
