using BlazorGolf.Core.Models;

namespace BlazorGolf.Client.Services
{
    public interface ICourseService
    {
         Task<IEnumerable<Course>> GetCoursesAsync();
         Task<Course> GetCourseAsync(Guid courseId);
         Task<Course> CreateCourseAsync(Course course);
         Task<Course> UpdateCourseAsync(Course course);
         Task DeleteCourseAsync(Course course);
    }
}