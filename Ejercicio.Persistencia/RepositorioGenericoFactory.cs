

namespace Ejercicio.Persistence
{
    using Ejercicio.Persistence.Interfaces;
    using Ejercicio.Persistence.Metadata;
    using Ejercicio.Trazas;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;

    public class RepositorioGenericoFactory : IRepositorioGenericoFactory
    {
        private IDatabaseConnectionFactory databaseConnectionFactory;
        private static List<string> propiedadesAuditoria = new List<string>() { "CreatedBy", "CreatedDate", "UpdateBy", "UpdatedDate" };
        private static string propiedadSoftDelete = "DeleteDate";
        private TrazaLoggerInterceptor trazaLoggerInterceptor;
        public RepositorioGenericoFactory(IDatabaseConnectionFactory databaseConnectionFactory, TrazaLoggerInterceptor trazaLoggerInterceptor)
        {
            this.databaseConnectionFactory = databaseConnectionFactory;
            this.trazaLoggerInterceptor = trazaLoggerInterceptor;
        }

        public IRepositorioGenerico<T> GetRepositorio<T>() where T : class
        {
            string nombreTabla = null;
            string aliasTabla = null;
            string Uid = null;
            List<string> propiedadesTabla = new List<string>();
            List<string> propiedadesTablaRequeridas = new List<string>();
            List<string> propiedadesTablaRequeridasSoloInsert = new List<string>();
            Dictionary<string, int> propiedadesTablaMaxLength = new Dictionary<string, int>();
            List<PropertyInfo> relaciones1a1 = new List<PropertyInfo>();
            List<PropertyInfo> relaciones1aMuchos = new List<PropertyInfo>();
            PropertyInfo clavePrincipal = null;
            PropertyInfo propiedadInfoUid = null;
            Type tipoEntidad = typeof(T);
            TableAttribute customAttribute = tipoEntidad.GetCustomAttribute<TableAttribute>();
            if (customAttribute != null)
            {
                nombreTabla = customAttribute.Name;
                aliasTabla = "_" + nombreTabla.Substring(0, nombreTabla.Length - 2);
            }

            AliasAttribute aliasAttribute = tipoEntidad.GetCustomAttribute<AliasAttribute>();
            if (aliasAttribute != null)
            {
                aliasTabla = aliasAttribute.Nombre;
            }

            
            List<PropertyInfo> listInfo = tipoEntidad.GetProperties().ToList();
            var listInfoPropiedadesTabla = new List<PropertyInfo>();
            listInfo.ForEach((PropertyInfo p) =>
            {
                if (p.GetCustomAttribute<NotMappedAttribute>() == null)
                {
                    if (Uid == null && p.PropertyType.FullName.Contains("System.Guid"))
                    {
                        Uid = p.Name;
                        propiedadInfoUid = p;
                    }

                    if (p.GetCustomAttribute<OneToOneAttribute>() != null)
                    {
                        relaciones1a1.Add(p);
                    }
                    else if (p.GetCustomAttribute<OneToManyAttribute>() != null)
                    {
                        relaciones1aMuchos.Add(p);
                    }
                    else if (p.GetCustomAttribute<KeyAttribute>() == null)
                    {
                        propiedadesTabla.Add(p.Name);
                        listInfoPropiedadesTabla.Add(p);
                        if (!p.PropertyType.FullName.Contains("hierarchyid"))
                        {
                            if (p.GetCustomAttribute<RequiredAttribute>() != null)
                            {
                                propiedadesTablaRequeridas.Add(p.Name);
                            }
                            if (p.GetCustomAttribute<RequiredOnlyInsertAttribute>() != null)
                            {
                                propiedadesTablaRequeridasSoloInsert.Add(p.Name);
                            }
                            var maxLen = p.GetCustomAttribute<MaxLengthAttribute>();
                            if (maxLen != null && maxLen.Length > 0)
                            {
                                propiedadesTablaMaxLength.Add(p.Name, maxLen.Length);
                            }
                        }
                    }
                    else
                    {
                        clavePrincipal = p;
                    }

                }
            });

            if (string.IsNullOrEmpty(nombreTabla) || clavePrincipal == null)
            {
                throw new ArgumentException("Definicion Tabla invalida. (Anotacion necesaria: Table y Key)");
            }

            bool softDelete = propiedadesTabla.Contains(propiedadSoftDelete);
            listInfoPropiedadesTabla = listInfoPropiedadesTabla.Where(i =>
            {
                var nombre = i.Name;
                if (nombre == propiedadSoftDelete) return false;
                if (propiedadesAuditoria.Contains(nombre)) return false;
                return true;
            }).ToList();

            var heredaAuditoria = propiedadesAuditoria.All(pa => propiedadesTabla.Contains(pa));

            IRepositorioGenerico<T> repositorioBase = new RepositorioGenerico<T>(clavePrincipal.Name, nombreTabla, aliasTabla, listInfoPropiedadesTabla, clavePrincipal, relaciones1a1, relaciones1aMuchos, databaseConnectionFactory, trazaLoggerInterceptor, heredaAuditoria, softDelete, propiedadesTablaRequeridas, propiedadesTablaRequeridasSoloInsert, propiedadesTablaMaxLength, Uid, propiedadInfoUid);
            return repositorioBase;
        }
    }
}
