using FluentValidation;
using BlazorGolf.Core.Extensions;
using BlazorGolf.Core.Models;

namespace BlazorGolf.Core.Models
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(x => x.CourseId).NotEmpty()
                              .NotNull()
                              .Length(36)
                              .WithMessage("Id must be a Guid");
            RuleFor(x => x.PartitionKey).NotEmpty().Equal("Course");
            RuleFor(x => x.ETag).NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.City).NotNull().NotEmpty().MinimumLength(3).MaximumLength(50);
            RuleFor(x => x.State).NotNull().NotEmpty().Must(x => x.IsValidEnumMember(typeof(USStates))).WithMessage("State must be a valid 2 character state abbreviation");
            RuleFor(x => x.Tees).NotNull().NotEmpty().Must(x => x.Count() > 0);
        }
    }
}
