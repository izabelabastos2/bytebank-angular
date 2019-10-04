using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;


namespace Vale.Geographic.Domain.Base
{
    [ExcludeFromCodeCoverage]
    public static class EnumExtensions
    {

        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type",
                    nameof(enumerationValue));
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0) return ((DescriptionAttribute)attrs[0]).Description;
            }

            return enumerationValue.ToString();
        }

        public static string GetLocalizedDescription<T>(this T enumerationValue) where T : struct
        {
            var fi = enumerationValue.GetType().GetField(enumerationValue.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            return enumerationValue.ToString();
        }
    }
}