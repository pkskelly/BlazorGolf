using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BlazorGolfApi.Entities;
using FluentValidation;
using BlazorGolfApi.Services;
using Ardalis.GuardClauses;

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
        private readonly IRepository<Course> _repository;
        public IValidator<Course> _courseValidator { get; }


        public CoursesController(IValidator<Course> courseValidator, IRepository<Course> repository, ILogger<CoursesController> logger)
        {
            Guard.Against.Null(courseValidator, nameof(courseValidator));
            Guard.Against.Null(repository, nameof(repository));
            Guard.Against.Null(logger, nameof(logger));

            _courseValidator = courseValidator;
            _logger = logger;
            _repository = repository;
        }

        // GET: api/courses 
        [HttpGet(Name = "GetCourses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation($"GetCourses called with no id");
            var courses = await _repository.GetAll();
            _logger.LogInformation("GetCourses returning");
            return Ok(courses);
        }

        // GET: api/courses/6394652d-b853-408a-a2aa-b3b59d8abf82
        [HttpGet("{id:guid}", Name = "GetCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation($"GetCourses called with id: {id.ToString()}");
            var course = await _repository.GetById(id.ToString());
            if (null == course)
            {
                _logger.LogWarning($"GetCourses called with invalid id: {id.ToString()}");
                return NotFound(
                    new
                    {
                        Error = $"Course with id {id.ToString()} not found"
                    }
                );
            }
            return Ok(course);
        }

        // POST api/courses
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Course course)
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
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"DeleteCourse called with id: {id.ToString()}");
            await _repository.Remove(id.ToString());
            return NoContent();
        }
    }
}