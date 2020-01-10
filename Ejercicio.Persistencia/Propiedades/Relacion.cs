
namespace Ejercicio.Persistencia.Propiedades
{
    using System.Collections.Generic;
    using System.Linq;

    public enum TipoRelacion
    {
        One,
        Many
    }

    public class Relacion
    {
        public string Id { get; private set; }
        public string Descripcion { get; private set; }
        public Tabla TablaPrincipal { get; private set; }
        public Tabla TablaSecundaria { get; private set; }
        public TipoRelacion Tipo { get; private set; }

        public string PropiedadJoinPrincipal { get; private set; }
        public string PropiedadJoinSecundaria { get; private set; }

        public Relacion(Tabla tablaPrincipal, Tabla tablaSecundaria, TipoRelacion tipo, string propiedadJoinPrincipal, string propiedadJoinSecundaria, string descripcion = "")
        {
            var id = string.Empty;
            switch (tipo)
            {
                case TipoRelacion.One:
                    id = tablaPrincipal.Alias + "_" + propiedadJoinPrincipal + tablaSecundaria.Alias;
                    var propiedadPrincipal = tablaPrincipal.Propiedades.Single(p => p.Nombre == propiedadJoinPrincipal);
                    Descripcion = propiedadPrincipal.DescripcionRelacionFK;
                    break;
                case TipoRelacion.Many:
                    id = tablaPrincipal.Alias + tablaSecundaria.Alias + "_" + propiedadJoinSecundaria;
                    var propiedadSecundaria = tablaSecundaria.Propiedades.Single(p => p.Nombre == propiedadJoinSecundaria);
                    Descripcion = propiedadSecundaria.DescripcionRelacionPK;
                    break;
                default:
                    break;
            }
            Id = id.ToLower();
            TablaPrincipal = tablaPrincipal;
            TablaSecundaria = tablaSecundaria;
            Tipo = tipo;
            PropiedadJoinPrincipal = propiedadJoinPrincipal;
            PropiedadJoinSecundaria = propiedadJoinSecundaria;
        }

        public override string ToString()
        {
            return Id;
        }

        public List<string> PropiedadesValidas()
        {
            return TablaPrincipal.PropiedadesValidas().Concat(TablaSecundaria.PropiedadesValidas()).ToList();
        }
    }
}
