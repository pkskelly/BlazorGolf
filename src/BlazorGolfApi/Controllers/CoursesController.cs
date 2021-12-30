using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BlazorGolfApi.Entities;
using FluentValidation;

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
        public IValidator<Course> _courseValidator { get; }

        public CoursesController(IValidator<Course> courseValidator, ILogger<CoursesController> logger)
        {
            _courseValidator = courseValidator;
            _logger = logger;
        }

        [HttpGet(Name = "GetCourses")]
        [Route("/api/courses/{id:guid?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Course>))]        
        public IActionResult Get(Guid? id)
        {
            var result = id.HasValue ? GetCourseById(id.Value) : GetCourses(); 
            return result;            
        }       

        private IActionResult GetCourses()
        {
            _logger.LogInformation($"GetCourses called with no id"); 
            var rng = new Random();
            var courses =  Enumerable.Range(1, 5).Select(index => new Course
            {
                Id = Guid.NewGuid().ToString(),
                Name = _courses[rng.Next(_courses.Length)],
                Slope = rng.Next(55, 155)
            })
            .ToArray();
            _logger.LogInformation("GetCourses returning");
            return Ok(courses);
        }


        private IActionResult GetCourseById(Guid id)
        {
            _logger.LogInformation($"GetCourses called with id: {id}"); 
            return Ok(new Course
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Chateau Elan - Woodlands",
                Slope = 55
            });
        }

        [HttpPost(Name = "CreateCourses")]
        [Route("/api/courses")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Course))]        
        public IActionResult Get(Course course)
        {
            _logger.LogInformation($"CreateCourse called with id: {course.Id}"); 
            var result = _courseValidator.Validate(course);
            if (result.IsValid)
            {
                return CreatedAtRoute("GetCourses", new { id = course.Id }, course);
            }
            else
            {
                return BadRequest(result.Errors);
            }           
        }        

        [HttpDelete(Name = "DeleteCourse")]
        [Route("/api/courses/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        public IActionResult Get(Guid id)
        {
            _logger.LogInformation($"DeleteCourse called with id: {id.ToString()}"); 
            return NoContent();
        }        
    }
}