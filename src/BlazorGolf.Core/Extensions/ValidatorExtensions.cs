using FluentValidation;
using FluentValidation.Validators;

namespace BlazorGolf.Core.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> IsPhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {  
            string phoneNumberRegEx =  @"^[\+]?[{1}]?[(]?[2-9]\d{2}[)]?[-\s\.]?[2-9]\d{2}[-\s\.]?[0-9]{4}$";
            return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(phoneNumberRegEx));
        }
    }
}