using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Freelance.Infrastructure.Utils
{
    public static class Extensions
    {
        public static T ConvertToFlagEnum<T>(this string[] flags) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException($"{typeof(T).ToString()} must be an enumerated type");

            int flagValue = 0;
            foreach (var val in flags)
            {
                int currentValue = (int)Enum.Parse(typeof(T), val);
                flagValue |= currentValue;
            }
            return (T)Enum.ToObject(typeof(T), flagValue);
        }

        public static string GetFirstCharacters(this string text, int amountOfCharacters)
        {
            if (amountOfCharacters < 0 || text.Length < amountOfCharacters)
            {
                return text;
            }

            return text.Substring(0, amountOfCharacters) + "...";
        }

        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?
                .GetMember(enumValue.ToString())?
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;
        }
    }
}