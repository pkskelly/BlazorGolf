using BlazorGolf.Core.Models;

namespace BlazorGolf.Client.Services
{
    public interface ICourseService
    {
         Task<IEnumerable<Course>> GetCourses();
         Task<Course> GetCourse(Guid courseId);
    }
}