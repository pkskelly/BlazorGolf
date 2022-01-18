using FluentValidation;
using FluentValidation.Validators;

namespace BlazorGolf.Core.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            string phoneNumberRegEx =  @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
            return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(phoneNumberRegEx));
        }
    }
}