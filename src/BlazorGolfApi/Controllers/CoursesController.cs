using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BlazorGolfApi.Entities;
using FluentValidation;

namespace BlazorGolfApi.Controllers
{
    [ApiController]
    [Route("api/courses")]
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

        // GET: api/courses 
        [HttpGet(Name="GetCourses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
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

        // GET: api/courses/38387939874-0003-30 
        [HttpGet("{id:guid}", Name="GetCourse")]
        public IActionResult Get(Guid id)
        {
           _logger.LogInformation($"GetCourses called with id: {id.ToString()}"); 
            return Ok(new Course
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Chateau Elan - Woodlands",
                Slope = 55
            });
        }

        // POST api/courses
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]     
        [ProducesResponseType(StatusCodes.Status400BadRequest)]     
        public ActionResult<Course> Post([FromBody] Course course)
        {
            _logger.LogInformation($"CreateCourse called with id: {course.Id}"); 
            var result = _courseValidator.Validate(course);
            if (result.IsValid)
            {
                return CreatedAtRoute("GetCourse", new { id = course.Id }, course);
            }
            else
            {
                return BadRequest(result.Errors);
            }           
        }
        
        // DELETE api/courses/6394652d-b853-408a-a2aa-b3b59d8abf82
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]     
        public IActionResult Delete(Guid id)
        {
            _logger.LogInformation($"DeleteCourse called with id: {id.ToString()}"); 
            return NoContent();
        }
    }
}