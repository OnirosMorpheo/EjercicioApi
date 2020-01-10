
namespace Ejercicio.Utilities.Extensions
{
    using System;
    public sealed class StringValueAttribute : Attribute
    {
        public string Value { get; private set; }

        public StringValueAttribute(string value)
        {
            this.Value = value;
        }
    }
}
