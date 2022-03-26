using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BlazorGolf.Core.Models;
using FluentValidation;
using BlazorGolf.Api.Services;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
namespace BlazorGolf.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/courses")]
    [EnableCors("AllowEveryone")]
    public class CoursesController : ControllerBase
    {

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
            _logger.LogInformation($"GetCourses called with no id!");
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
            _logger.LogInformation($"GetCourse called with id: {id.ToString()}");
            var course = await _repository.GetById(id.ToString());
            if (course == null)
            {
                _logger.LogWarning($"GetCourse called with invalid id: {id.ToString()}");
                return NotFound(
                    new
                    {
                        Error = $"Course with id {id.ToString()} not found!"
                    }
                );
            }
            return Ok(course);
        }

        // GET: api/courses/6394652d-b853-408a-a2aa-b3b59d8abf82/tees
        [HttpGet("{id:guid}/tees", Name = "GetTees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTees(Guid id)
        {
            _logger.LogInformation($"GetTees called with id: {id.ToString()}");
            var course = await _repository.GetById(id.ToString());
            if (course == null)
            {
                _logger.LogWarning($"GetTees called with invalid id: {id.ToString()}");
                return NotFound(
                    new
                    {
                        Error = $"Course with id {id.ToString()} not found!"
                    }
                );
            }
            return Ok(course.Tees);
        }

        // POST api/courses
        [HttpPost(Name = "CreateCourse")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] Course course)
        {

            _logger.LogInformation($"CreateCourse called with id: {course.CourseId}");
            var result = _courseValidator.Validate(course);
            try
            {
                if (result.IsValid)
                {
                    await _repository.Create(course);
                    return CreatedAtRoute("GetCourse", new { id = course.CourseId }, course);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"CreateCourse failed with exception: {ex.Message}");
                return BadRequest(
                    new
                    {
                        Error = $"Course with id {course.CourseId} already exists!"
                    }
                );
            }
        }

        // PUT api/courses
        [HttpPut("{id:guid}", Name = "UpdateCourse")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] Course course)
        {
            _logger.LogInformation($"Update called with id: {id}");
            if (course == null)
            {
                return BadRequest(
                    new
                    {
                        Error = $"Course must not be null!"
                    }
                );
            }
            try
            {
                var providedCourseGuid = new Guid(course.CourseId);
                if (id != providedCourseGuid)
                {
                    _logger.LogWarning($"URI path id {id} does not match body course id {course.CourseId}!");
                    return BadRequest(
                        new
                        {
                            Error = $"URI path id {id} does not match body course id {course.CourseId}!"
                        }
                    );
                }
                var existingCourse = await _repository.GetById(id.ToString());
                if (existingCourse == null)
                {
                    return NotFound(
                        new
                        {
                            Error = $"Course with id {id} not found!"
                        }
                    );
                }
                var result = _courseValidator.Validate(course);

                if (result.IsValid)
                {
                    await _repository.Update(course);
                    return AcceptedAtRoute("UpdateCourse", new {id = course.CourseId }, course);
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"UpdateCourse failed with exception: {ex.Message}");
                return BadRequest(
                    new
                    {
                        Error = $"Error updating course with id {course.CourseId}!"
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateCourse failed with exception: {ex.Message}");
                return BadRequest(
                    new
                    {
                        Error = $"Error updating course with id {course.CourseId}!"
                    }
                );
            }
        }

        // DELETE api/courses/6394652d-b853-408a-a2aa-b3b59d8abf82
        [Authorize(Roles = "weather.admin")]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"DeleteCourse called with id: {id.ToString()}");
            await _repository.Delete(id.ToString());
            return NoContent();
        }
    }
}