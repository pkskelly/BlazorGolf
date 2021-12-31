using System.ComponentModel;
using BlazorGolf.Core.Models;

namespace BlazorGolf.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string FullStateName(this USStates enumValue)
        {
            var enumType = typeof(USStates);
            var memberData = enumType.GetMember(enumValue.ToString());
            var description = (memberData[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute).Description;
            return description;
        }

        public static bool IsValidEnumMember(this string testString, Type enumType)
        {
            return Enum.IsDefined(enumType,testString);
        }
    }
}