

namespace Ejercicio.Persistencia.Metadata
{
    using System;
    public class PropDescriptionAttribute : Attribute
    {
        public string Nombre { get; set; }

        public PropDescriptionAttribute(string nombre)
        {
            this.Nombre = nombre;
        }
    }
}

