using BlazorGolf.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorGolf.Api.Services
{
    public class CourseRespository : IRepository<Course>
    {
        private readonly ApplicationContext _context;

        public CourseRespository(ApplicationContext applcationContext)
        {
            _context = applcationContext;
            // _context.Database.EnsureDeleted();
             _context.Database.EnsureCreated();
        }

        public async Task<Course> Create(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetById(string id)
        {
            return await _context.Courses.SingleOrDefaultAsync<Course>(c => c.CourseId == id);
        }

        public async Task Delete(string id)
        {
            var course = await _context.Courses.SingleOrDefaultAsync<Course>(c => c.CourseId == id);
            _context.Remove<Course>(course);
             await _context.SaveChangesAsync();
             return;
        }

        public async Task<Course> Update(Course course)
        {
            var foundCourse =  await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == course.CourseId);

            if (foundCourse != null)
            {
                foundCourse.Name = course.Name;
                foundCourse.City = course.City;
                foundCourse.Phone = course.Phone;
                foundCourse.State = course.State;
                foundCourse.Tees = course.Tees;
                _context.SaveChanges();
                return foundCourse;
            }
            return null;
        }
    }
}