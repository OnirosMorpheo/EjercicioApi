

namespace Ejercicio.Persistence.Metadata
{
    using System;
    public class AliasAttribute : Attribute
    {
        public string Nombre { get; set; }

        public AliasAttribute(string nombre)
        {
            this.Nombre = nombre;
        }
    }
}
