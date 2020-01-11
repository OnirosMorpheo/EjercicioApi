

namespace Ejercicio.Persistence.Metadata
{
    using System;
    public class OneToManyAttribute : Attribute
    {
        public string Clave;
        public string ClaveRelacionada;

        public OneToManyAttribute(string clave, string claveRelacionada)
        {
            this.Clave = clave;
            this.ClaveRelacionada = claveRelacionada;
        }
    }
}
