using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Courses;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Courses;

namespace learnova.LearningService.Application.Handlers.Courses
{
    public class GetCourseByIdHandler
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseByIdHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDto?> HandleAsync(GetCourseByIdQuery query, CancellationToken cancellationToken = default)
        {
            if (query == null) throw new System.ArgumentNullException(nameof(query));

            var course = await _courseRepository.GetByIdAsync(query.Id, cancellationToken);
            if (course == null) return null;

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
