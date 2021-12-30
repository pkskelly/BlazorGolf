using FluentValidation;

namespace BlazorGolfApi.Entities
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .NotNull()
                              .Length(36)
                              .WithMessage("Id must be a Guid");
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Slope).NotEmpty().GreaterThanOrEqualTo(55).LessThanOrEqualTo(155);
            //RuleFor(x => x.Rating).NotEmpty().GreaterThan(69.0);  
            //RuleFor(x => x.Rating).ScalePrecision(1, 3);
          
        }
    } 
}
