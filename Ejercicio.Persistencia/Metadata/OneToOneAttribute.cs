

namespace Ejercicio.Persistencia.Metadata
{
    using System;
    public class OneToOneAttribute : Attribute
    {
        public string Clave;
        public string ClaveRelacionada;

        public OneToOneAttribute(string clave, string claveRelacionada)
        {
            this.Clave = clave;
            this.ClaveRelacionada = claveRelacionada;
        }
    }
}
