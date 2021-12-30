using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentAssertions;
using BlazorGolfApi.Entities;
using Moq;
using BlazorGolfApi.Services;
using BlazorGolfApi.Controllers;
using Microsoft.Extensions.Logging;
using System;
using Bogus;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;

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
            var courses = new Faker<Course>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode if you prefer.
                .StrictMode(true)
                .RuleFor(c => c.Id, Guid.NewGuid().ToString())
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.Slope, f => f.Random.Number(55, 155))
                .Generate(5);

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
            var course = new Faker<Course>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode if you prefer.
                .StrictMode(true)
                .RuleFor(c => c.Id, Guid.NewGuid().ToString())
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.Slope, f => f.Random.Number(55, 155))
                .Generate();

            _courseRepository.Setup(x => x.GetById(course.Id)).ReturnsAsync(course);
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            //Act
            var result = await controller.Get(Guid.Parse(course.Id));
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
            var course = new Faker<Course>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode if you prefer.
                .StrictMode(true)
                .RuleFor(c => c.Id, Guid.NewGuid().ToString())
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.Slope, f => f.Random.Number(55, 155))
                .Generate();

            _courseRepository.Setup(x => x.GetById(course.Id)).Returns(Task.FromResult<Course>(null));
            var controller = new CoursesController(_validator, _courseRepository.Object, _logger.Object);
            //Act
            var result = await controller.Get(Guid.Parse(course.Id));
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

            _courseRepository.Setup(x => x.Remove(courseId)).Returns(Task.CompletedTask);
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
            var course = new Faker<Course>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode if you prefer.
                .StrictMode(true)
                .RuleFor(c => c.Id, Guid.NewGuid().ToString())
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.Slope, f => f.Random.Number(55, 155))
                .Generate();

            _courseRepository.Setup(x => x.Add(course)).ReturnsAsync(course);
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
    }
}




