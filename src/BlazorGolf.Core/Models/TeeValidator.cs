using FluentValidation;

namespace BlazorGolf.Core.Models
{
    public class TeeValidator : AbstractValidator<Tee>
    {
        public TeeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(5).MaximumLength(100);
            RuleFor(x => x.Par).GreaterThanOrEqualTo(59).LessThanOrEqualTo(75);
            RuleFor(x => x.Slope).GreaterThanOrEqualTo(55).LessThanOrEqualTo(155);
            RuleFor(x => x.Rating).GreaterThanOrEqualTo(59.0).LessThanOrEqualTo(130.0);
            RuleFor(x => x.BogeyRating).GreaterThanOrEqualTo(59.0).LessThanOrEqualTo(130.0);

            RuleFor(x => x.FrontNineRating).GreaterThanOrEqualTo(24.0).LessThanOrEqualTo(40.0);
            RuleFor(x => x.FrontNineSlope).GreaterThanOrEqualTo(55).LessThanOrEqualTo(155);
            RuleFor(x => x.BackNineRating).GreaterThanOrEqualTo(24.0).LessThanOrEqualTo(40.0);
            RuleFor(x => x.BackNineSlope).GreaterThanOrEqualTo(55).LessThanOrEqualTo(155);
        }
    }
}