
namespace Ejercicio.Utilities.Extensions
{
    using System;

    public static class EnumExtension
    {
        public static string ToStringValue(this Enum value)
        {
            StringValueAttribute[] attributes = (StringValueAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false);
            return ((attributes != null) && (attributes.Length > 0)) ? attributes[0].Value : value.ToString();
        }
    }
}
