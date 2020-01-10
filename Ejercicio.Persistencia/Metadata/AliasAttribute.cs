

namespace Ejercicio.Persistencia.Metadata
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
