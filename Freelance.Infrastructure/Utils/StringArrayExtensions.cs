using System;

namespace Freelance.Infrastructure.Utils
{
    public static class StringArrayExtensions
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
    }
}