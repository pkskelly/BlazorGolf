using FluentValidation;

namespace BlazorGolf.Core.Models
{
    public class TeeValidator : AbstractValidator<Tee>
    {
        public TeeValidator()
        {
             RuleFor(x => x.TeeId).NotEmpty()
                              .NotNull()
                              .Length(36)
                              .WithMessage("Id must be a Guid");
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(x => x.Par).GreaterThanOrEqualTo(59).LessThanOrEqualTo(75);

            RuleFor(x => x.Slope).GreaterThanOrEqualTo(55).LessThanOrEqualTo(155);
            RuleFor(x => x.FrontNineSlope).GreaterThanOrEqualTo(55).LessThanOrEqualTo(155);
            RuleFor(x => x.BackNineSlope).GreaterThanOrEqualTo(55).LessThanOrEqualTo(155);

            RuleFor(x => x.Rating).GreaterThanOrEqualTo(59.0).LessThanOrEqualTo(130.0);
            RuleFor(x => x.BogeyRating).GreaterThanOrEqualTo(59.0).LessThanOrEqualTo(130.0);
            RuleFor(x => x.FrontNineRating).GreaterThanOrEqualTo(27.5).LessThanOrEqualTo(65.0);
            RuleFor(x => x.BackNineRating).GreaterThanOrEqualTo(27.5).LessThanOrEqualTo(65.0);

        }
    }
}