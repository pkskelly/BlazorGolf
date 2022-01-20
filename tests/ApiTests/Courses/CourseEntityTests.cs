using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using System;
using BlazorGolf.Core.Models;

namespace ApiTests.Courses
{
    [TestFixture]
    public class CourseEntityTests
    {

        private CourseValidator? _courseValidator;
        private TeeValidator? _teeValidator;


        [SetUp]
        public void Setup()
        {
            _courseValidator = new CourseValidator();
            _teeValidator = new TeeValidator();
        }

        [Test]
        public void Course_DefaultShouldHave_NewGuid()
        {
            var course = new Course();
            var result = _courseValidator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.CourseId);
            Assert.AreEqual(36, course.CourseId.Length);
        }

        [Test]
        public void Course_ShouldHaveError_NonGuid()
        {
            var course = new Course();
            course.CourseId = "this is not a guid";
            var result = _courseValidator.TestValidate(course);

            result.ShouldHaveValidationErrorFor(x => x.CourseId);
        }

        [Test]
        public void Course_DefaultShouldNotHave_EmptyorNullName()
        {
            var course = new Course();
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Course_ShouldHave_DefaultPartitionKey()
        {
            var course = new Course();
            var result = _courseValidator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.PartitionKey);
        }

        [Test]
        public void Course_ShouldThrow_MissingPartitionKey()
        {
            var course = new Course();
            course.PartitionKey = "";
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.PartitionKey);
        }

        [Test]
        public void Course_ShouldThrow_NullPartitionKey()
        {
            var course = new Course();
            course.PartitionKey = null;
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.PartitionKey);
        }

        [Test]
        public void Course_WithEmptyCity_Fails()
        {
            var course = new Course();
            course.City = "";
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithNullCity_Fails()
        {
            var course = new Course();
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithLongCity_Fails()
        {
            var course = new Course();
            course.City = "This is a very long city name that is more than 50 characters long";
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithShortCity_Fails()
        {
            var course = new Course();
            course.City = "ci";
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithCity_IsValid()
        {
            var course = new Course();
            course.City = "London";            
            var result = _courseValidator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.City);
        }

        [Test]
        public void Course_WithStateAbbreviation_Succeeds()
        {
            var course = new Course();
            course.State = "AL";
            var result = _courseValidator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.State);
        }

        [Test]
        public void Course_WithMissingStateAbbreviation_Fails()
        {
            var course = new Course();
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.State);
        }

        [Test]
        public void Course_WithMissingPhone_Fails()
        {
            var course = new Course();
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [TestCase("e")]
        [TestCase("123654")]
        [TestCase("123 456-78907")]
        [TestCase("123-456-78907")]
        public void Course_WithInvalidPhone_Fails( string phoneNumber)
        {
            var course = new Course();
            course.Phone = phoneNumber;
            var result = _courseValidator.TestValidate(course);
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [TestCase("678 555-1212")]
        [TestCase("(223) 456-7890")]
        [TestCase("223-456-7890")]
        [TestCase("12345678907")]
        public void Course_WithValidPhone_Succeeds(string phoneNumber)
        {
            var course = new Course();
            course.Phone = phoneNumber;
            var result = _courseValidator.TestValidate(course);
            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }


        [Test]
        public void Tee_DefaultNewObject_Fails()
        {
            var tee = new Tee();
            var result = _teeValidator.TestValidate(tee);
            Assert.IsFalse(result.IsValid);
            Assert.GreaterOrEqual(10, result.Errors.Count);
        }

        [Test]
        public void Tee_DefaultShouldHave_NewGuid()
        {
            var tee = new Tee();
            var result = _teeValidator.TestValidate(tee);
            result.ShouldNotHaveValidationErrorFor(x => x.TeeId);
            Assert.AreEqual(36, tee.TeeId.Length);
        }

        [Test]
        public void Tee_ShouldHaveError_NonGuid()
        {
            var tee = new Tee();
            tee.TeeId = "this is not a guid";
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.TeeId);
        }

        [Test]
        public void Tee_NameGreaterThan100_Fails()
        {
            var tee = new Tee();
            tee.Name = "This is a very long name that is more than 100 characters long. This is a very long name that is more than 100 characters long. This is a very long name that is more than 100 characters long.";
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Tee_NameLessThan4_Fails()
        {
            var tee = new Tee();
            tee.Name = "Thi";
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Test]
        public void Tee_ParLessThan59_Fails()
        {
            var tee = new Tee();
            tee.Par = 58;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Par);
        }

        [Test]
        public void Tee_ParGreaterThan75_Fails()
        {
            var tee = new Tee();
            tee.Par = 76;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Par);
        }

        [Test]
        public void Tee_SlopeGreaterThan155_Fails()
        {
            var tee = new Tee();
            tee.Slope = 156;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Slope);
        }

        [Test]
        public void Tee_SlopeLessThan55_Fails()
        {
            var tee = new Tee();
            tee.Slope = 54;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Slope);
        }

        [Test]
        public void Tee_RatingLessThan59_Fails()
        {
            var tee = new Tee();
            tee.Rating = 58.9;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Rating);
        }

        [Test]
        public void Tee_RatingGreaterThan130_Fails()
        {
            var tee = new Tee();
            tee.Rating = 130.1;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.Rating);
        }

        [Test]
        public void Tee_BogeyRatingLessThan59_Fails()
        {
            var tee = new Tee();
            tee.BogeyRating = 58.9;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.BogeyRating);
        }

        [Test]
        public void Tee_BogeyRatingGreaterThan130_Fails()
        {
            var tee = new Tee();
            tee.BogeyRating = 130.1;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.BogeyRating);
        }

        [Test]
        public void Tee_FrontNineRatingLessThan59_Fails()
        {
            var tee = new Tee();
            tee.Name = "Sample passing name";
            tee.FrontNineRating = 58.9;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.FrontNineRating);
        }

        [Test]
        public void Tee_FrontNineRatingGreaterThan130_Fails()
        {
            var tee = new Tee();
            tee.FrontNineRating = 130.1;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.FrontNineRating);
        }

        [Test]
        public void Tee_FrontNineSlopeGreaterThan155_Fails()
        {
            var tee = new Tee();
            tee.FrontNineSlope = 156;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.FrontNineSlope);
        }

        [Test]
        public void Tee_FrontNineSlopeLessThan55_Fails()
        {
            var tee = new Tee();
            tee.FrontNineSlope = 54;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.FrontNineSlope);
        }

        [Test]
        public void Tee_BackNineRatingLessThan59_Fails()
        {
            var tee = new Tee();
            tee.BackNineRating = 58.9;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.BackNineRating);
        }

        [Test]
        public void Tee_BackNineRatingGreaterThan130_Fails()
        {
            var tee = new Tee();
            tee.BackNineRating = 130.1;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.BackNineRating);
        }

        [Test]
        public void Tee_BackNineSlopeGreaterThan155_Fails()
        {
            var tee = new Tee();
            tee.BackNineSlope = 156;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.BackNineSlope);
        }

        [Test]
        public void Tee_BackNineSlopeLessThan55_Fails()
        {
            var tee = new Tee();
            tee.BackNineSlope = 54;
            var result = _teeValidator.TestValidate(tee);
            result.ShouldHaveValidationErrorFor(x => x.BackNineSlope);
        }
    }
}



