using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BlazorGolfApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowEveryone")]
    public class CoursesController : ControllerBase
    {
        private static readonly string[] _courses = new[]
        {
        "Reunion", "Hamilton Mill", "Chateau Elan - Woodlands", "Collins Hill", "Chiocopee Woods"
        };

        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ILogger<CoursesController> logger)
        {
            _logger = logger;
        }


        [HttpGet(Name = "GetCourses")]
        public IEnumerable<Course> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Course
            {
                Id = Guid.NewGuid().ToString(),
                Name = _courses[rng.Next(_courses.Length)]
            })
            .ToArray();
        }
    }
}