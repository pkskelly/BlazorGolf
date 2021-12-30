using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using BlazorGolfApi.Entities;

namespace ApiTests
{
    [TestFixture]
    public class CourseEntityTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Course_DefaultShouldHave_NewGuid()
        {
            Assert.Pass();
        }
 
    }
}



