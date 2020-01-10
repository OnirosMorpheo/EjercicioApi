

namespace Ejercicio.Persistencia.Metadata
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Reflection;
    public static class TypeExtension
    {
        public static string NombreTabla(this Type tipoEntidad)
        {
            TableAttribute customAttribute = tipoEntidad.GetCustomAttribute<TableAttribute>();
            if (customAttribute != null)
            {
                return customAttribute.Name;
            }
            return string.Empty;
        }

        public static string PropiedadDescripcionTabla(this Type tipoEntidad)
        {
            PropDescriptionAttribute customAttribute = tipoEntidad.GetCustomAttribute<PropDescriptionAttribute>();
            if (customAttribute != null)
            {
                return customAttribute.Nombre;
            }
            return string.Empty;
        }
    }
}
