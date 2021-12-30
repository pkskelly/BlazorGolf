using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using BlazorGolfApi.Entities;

namespace ApiTests
{
    [TestFixture]
    public class CourseEntityTests
    {

        private CourseValidator? _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CourseValidator();
        }

        [Test]
        public void Course_DefaultShouldHave_NewGuid()
        {
            var course = new Course();
            var result = _validator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
            Assert.AreEqual(36, course.Id.Length);
        }

        [Test]
        public void Course_ShouldHaveError_NonGuid()
        {
            var course = new Course();
            course.Id = "this is not a guid";
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Test]
        public void Course_DefaultShouldNotHave_EmptyorNulltName()
        {
            var course = new Course();
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Course_ShouldHave_DefaultSlope()
        {
            var course = new Course();
            course.Slope = 54;
            course.Name = "Chateau Elan - Woodlands";
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.Slope);
        }

    }
}



