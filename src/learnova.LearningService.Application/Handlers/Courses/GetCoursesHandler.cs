using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using learnova.LearningService.Application.DTOs.Courses;
using learnova.LearningService.Application.Interfaces;
using learnova.LearningService.Application.Queries.Courses;

namespace learnova.LearningService.Application.Handlers.Courses
{
    public class GetCoursesHandler
    {
        private readonly ICourseRepository _courseRepository;

        public GetCoursesHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<IEnumerable<CourseDto>> HandleAsync(GetCoursesQuery query, CancellationToken cancellationToken = default)
        {
            var courses = await _courseRepository.GetAllAsync(cancellationToken);

            return courses.Select(course => new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Slug = course.Slug,
                IsPublished = course.IsPublished,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt
            });
        }
    }
}
