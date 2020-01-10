

namespace Ejercicio.Persistencia.Propiedades
{
    using System.Collections.Generic;
    using System.Linq;
    public class Tabla
    {
        public string Nombre { get; set; }
        public string Alias { get; set; }
        public List<Propiedad> Propiedades { get; set; }
        public List<Relacion> Relaciones { get; set; }

        public Tabla(string alias, string nombre, List<Propiedad> propiedades = null)
        {
            this.Nombre = nombre;
            this.Alias = alias;
            Propiedades = propiedades ?? new List<Propiedad>();
            Relaciones = new List<Relacion>();
        }

        public override string ToString()
        {
            return Nombre;
        }

        public List<string> PropiedadesValidas(string alias = "")
        {
            if (string.IsNullOrEmpty(alias))
                alias = Alias;
            return Propiedades.Select(p => (alias + "." + p.Nombre).ToLower()).ToList();
        }
    }
}
