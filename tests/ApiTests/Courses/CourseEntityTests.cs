using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using BlazorGolfApi.Entities;
using System;
using BlazorGolf.Core.Models;

namespace ApiTests.Courses
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
            result.ShouldNotHaveValidationErrorFor(x => x.CourseID);
            Assert.AreEqual(36, course.CourseID.Length);
        }

        [Test]
        public void Course_ShouldHaveError_NonGuid()
        {
            var course = new Course();
            course.CourseID = "this is not a guid";
            var result = _validator.TestValidate(course);

            result.ShouldHaveValidationErrorFor(x => x.CourseID);
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

        [Test]
        public void Course_ShouldHave_DefaultPartitionKey()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            var result = _validator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.PartitionKey);
        }

        [Test]
        public void Course_ShouldThrow_MissingPartitionKey()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.PartitionKey = "";
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.PartitionKey);
        }

        [Test]
        public void Course_ShouldThrow_NullPartitionKey()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.PartitionKey = null;
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.PartitionKey);
        }

        [Test]
        public void Course_WithEmptyCity_Fails()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.City = "";
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithNullCity_Fails()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithLongCity_Fails()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.City = "This is a very long city name that is more than 50 characters long";
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithShortCity_Fails()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.City = "ci";
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithCity_IsValid()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.City = "London";            
            var result = _validator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithStateAbbreviation_Succeeds()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.City = "Birmingham";
            course.State = "AL";
            var result = _validator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.State);
        }

        [Test]
        public void Course_WithMissingStateAbbreviation_Fails()
        {
            var course = new Course();
            course.Slope = 55;
            course.Name = "Chateau Elan - Woodlands";
            course.ETag = Guid.NewGuid().ToString();
            course.City = "Birmingham";
            var result = _validator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.State);
        }
    }
}



