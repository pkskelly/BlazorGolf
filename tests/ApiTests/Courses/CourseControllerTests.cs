using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentAssertions;
using BlazorGolf.Core.Models;
using Moq;
using BlazorGolf.Api.Services;
using BlazorGolf.Api.Controllers;
using Microsoft.Extensions.Logging;
using System;
using Bogus;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ApiTests.Courses
{
    [TestFixture]
    public class CourseControllerTests
    {
        private readonly Mock<IRepository<Course>> _courseRepository = new Mock<IRepository<Course>>();
        private readonly Mock<ILogger<CoursesController>> _logger = new Mock<ILogger<CoursesController>>();
        private readonly IValidator<Course> _validator = new CourseValidator();

        [SetUp]
        public void Setup()
        {
        }

        public void CourseController_HasAllDependencies_Succeeds()
        {
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.Should().NotBeNull();
        }

        [Test]
        public void CourseContoller_NullLogger_ShouldThrowArgumentNullException()
        {
            Action act = () => new CoursesController(_validator, _courseRepository.Object, null);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("logger");
        }

        [Test]
        public void CourseContoller_NullRepository_ShouldThrowArgumentNullException()
        {
            
            Action act = () => new CoursesController(_validator, null, _logger.Object);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("repository");
        }

        [Test]
        public void CourseContoller_NullValidator_ShouldThrowArgumentNullException()
        {
            Action act = () => new CoursesController(null, _courseRepository.Object, _logger.Object);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("courseValidator");
        }

        [Test]
        public async Task CourseController_GetCourses_ReturnsCourses()
        {
            //Arrange 
            var courses = CourseHelpers.GetFakeCourses(5);

            _courseRepository.Setup(x => x.GetAll()).ReturnsAsync(courses);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            //Act
            var result = await controller.Get();
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(200);
            ((OkObjectResult)result).Value.Should().BeOfType<List<Course>>();
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(courses);
        }

        [Test]
        public async Task CourseController_GetCourse_ReturnsCourse()
        {
            //Arrange 
            var course = CourseHelpers.GetFakeCourse();

            _courseRepository.Setup(x => x.GetById(course.CourseId)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            //Act
            var result = await controller.Get(Guid.Parse(course.CourseId));
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(200);
            ((OkObjectResult)result).Value.Should().BeOfType<Course>();
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(course);
        }

        [Test]
        public async Task CourseController_GetCourseBadCourseId_ReturnsNoFound()
        {
            //Arrange 
            var course = CourseHelpers.GetFakeCourse();

            _courseRepository.Setup(x => x.GetById(course.CourseId)).Returns(Task.FromResult<Course>(null));
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            //Act
            var result = await controller.Get(Guid.Parse(course.CourseId));
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).StatusCode.Should().Be(404);
        }

        [Test]
        public async Task CourseController_DeleteCourse_Returns204()
        {
            //Arrange
            var courseId = Guid.NewGuid().ToString();

            _courseRepository.Setup(x => x.Delete(courseId)).Returns(Task.CompletedTask);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            //Act
            var result = await controller.Delete(Guid.Parse(courseId));
            //Assert
            result.Should().BeAssignableTo<NoContentResult>();
            ((NoContentResult)result).StatusCode.Should().Be(204);
        }

        [Test]
        public async Task CourseController_PostCourse_Returns201()
        {
            //Arrange
            Course course = CourseHelpers.GetFakeCourse();
            _courseRepository.Setup(x => x.Create(course)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.Post(course);
            //Assert
            result.Should().BeAssignableTo<CreatedAtRouteResult>();
            ((CreatedAtRouteResult)result).StatusCode.Should().Be(201);
            ((CreatedAtRouteResult)result).Value.Should().BeOfType<Course>();
            ((CreatedAtRouteResult)result).Value.Should().BeEquivalentTo(course);
        }

        [Test]
        public async Task CourseController_PutCourse_Returns202()
        {
            //Arrange
            Course course = CourseHelpers.GetFakeCourse();
            _courseRepository.Setup(x => x.GetById(course.CourseId)).ReturnsAsync(course);
            _courseRepository.Setup(x => x.Update(course)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.Put(new Guid(course.CourseId), course);
            //Assert
            result.Should().BeAssignableTo<AcceptedAtRouteResult>();
            ((AcceptedAtRouteResult)result).StatusCode.Should().Be(202);
            ((AcceptedAtRouteResult)result).Value.Should().BeOfType<Course>();
            ((AcceptedAtRouteResult)result).Value.Should().BeEquivalentTo(course);
        }

        [Test]
        public async Task CourseController_PutNullCourse_ReturnsBadRequest()
        {
            //Arrange
            Course course = CourseHelpers.GetFakeCourse();
            _courseRepository.Setup(x => x.Update(course)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.Put(Guid.NewGuid(), course);
            //Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            ((BadRequestObjectResult)result).StatusCode.Should().Be(400);
        }

        [Test]
        public async Task CourseController_PutMismatchCourseIds_ReturnsBadRequest()
        {
            //Arrange
            var testGuid = Guid.NewGuid();
            Course course = CourseHelpers.GetFakeCourse();
            var errorMessage = $"URI path id {testGuid.ToString()} does not match body course id {course.CourseId}!";
            _courseRepository.Setup(x => x.GetById(testGuid.ToString())).ReturnsAsync(course);
            _courseRepository.Setup(x => x.Update(course)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.Put(testGuid, course);
            //Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            ((BadRequestObjectResult)result).StatusCode.Should().Be(400);
            var props = CourseHelpers.GetDynamicProperties(((BadRequestObjectResult)result).Value);
            props["Error"].Should().Be(errorMessage);
        }

        [Test]
        public async Task CourseController_PutCourseIdNotInDatabase_ReturnsNotFound()
        {
            //Arrange
            Course course = CourseHelpers.GetFakeCourse();
            Course nullCourse = null;
            _courseRepository.Setup(x => x.GetById(course.CourseId)).ReturnsAsync(nullCourse);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.Put(new Guid(course.CourseId), course);
            //Assert
            result.Should().BeAssignableTo<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).StatusCode.Should().Be(404);
            var props = CourseHelpers.GetDynamicProperties(((NotFoundObjectResult)result).Value);
            props["Error"].Should().Be($"Course with id {course.CourseId} not found!");
        }

        [Test]
        public async Task CourseController_PutInvalidCourse_ReturnsBadRequest()
        {
            //Arrange
            Course course = CourseHelpers.GetFakeCourse();
            course.Phone = "123459";
            _courseRepository.Setup(x => x.GetById(course.CourseId)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.Put(new Guid(course.CourseId), course);
            //Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            ((BadRequestObjectResult)result).StatusCode.Should().Be(400);
        }


        [Test]
        public async Task CourseController_ValidCourseTeesEndpoint_Returns200()
        {
            //Arrange
               Course course = CourseHelpers.GetFakeCourse();
            _courseRepository.Setup(x => x.GetById(course.CourseId)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.GetTees(new Guid(course.CourseId));
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            ((OkObjectResult)result).StatusCode.Should().Be(200);
            ((OkObjectResult)result).Value.Should().BeOfType<List<Tee>>();
            ((OkObjectResult)result).Value.Should().BeEquivalentTo(course.Tees);
        }

        [Test]
        public async Task CourseController_InvalidCourseTeesEndpoint_Returns404()
        {
            //Arrange
            Course nullCourse = null;
            var fakeCourseId = Guid.NewGuid();
            _courseRepository.Setup(x => x.GetById(fakeCourseId.ToString())).ReturnsAsync(nullCourse);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //Act
            var result = await controller.GetTees(fakeCourseId);
            //Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<NotFoundObjectResult>();
            ((NotFoundObjectResult)result).StatusCode.Should().Be(404);
            var props = CourseHelpers.GetDynamicProperties(((NotFoundObjectResult)result).Value);
            props["Error"].Should().Be($"Course with id {fakeCourseId.ToString()} not found!");
        }
    }
}
