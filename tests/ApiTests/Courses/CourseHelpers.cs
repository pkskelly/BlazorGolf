using System;
using System.Collections.Generic;
using System.Linq;
using BlazorGolf.Core.Models;
using Bogus;

namespace ApiTests.Courses
{
    internal class CourseHelpers {

        internal static List<Course>? GetFakeCourses(int numberOfCourses)
        {
            var courses = new Faker<Course>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode if you prefer.
                //.StrictMode(true)
                .RuleFor(c => c.CourseId, Guid.NewGuid().ToString())
                .RuleFor(c => c.PartitionKey, "Course")
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.ETag, Guid.NewGuid().ToString())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.State, f =>  f.Address.StateAbbr())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumberFormat())
                .Generate(numberOfCourses);
            return courses;
        }

        internal static Course GetFakeCourse()
        {
            var course = new Faker<Course>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode if you prefer.
                //.StrictMode(true)
                .RuleFor(c => c.CourseId, Guid.NewGuid().ToString())
                .RuleFor(c => c.PartitionKey, "Course")
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.ETag, Guid.NewGuid().ToString())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.State, f => f.Address.StateAbbr())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumberFormat())
                .RuleFor(c => c.Tees, f => new List<Tee>()
                {
                    new Tee()
                    {
                        TeeId = Guid.NewGuid().ToString(),
                        Name = "Blue",
                        Par = 72,
                        Slope = f.Random.Int(55,155),
                        Rating = f.Random.Double(59.0, 74.0),
                        BogeyRating  = 104.0,
                        FrontNineRating = 34.0,
                        FrontNineSlope = 121,
                        BackNineRating = 36.0,
                        BackNineSlope  = 124
                    },
                    new Tee()
                    {
                        TeeId = Guid.NewGuid().ToString(),
                        Name = "White",
                        Par = 72,
                        Slope = f.Random.Int(55,155),
                        Rating = f.Random.Double(59.0, 74.0),
                        BogeyRating  = 104.0,
                        FrontNineRating = 34.0,
                        FrontNineSlope = 121,
                        BackNineRating = 36.0,
                        BackNineSlope  = 124
                    }
                })
                .Generate();
            return course;
        }

        internal static Dictionary<string, object?> GetDynamicProperties(object resultValue)
        {
            return (resultValue.GetType()
                               .GetProperties()
                               .ToDictionary(p => p.Name, p => p.GetValue(resultValue)));
        }
    }
}