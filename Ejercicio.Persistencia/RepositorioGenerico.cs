

namespace Ejercicio.Persistencia
{
    using Ejercicio.Persistencia.Interfaces;
    using Ejercicio.Utilities;
    using Ejercicio.Utilities.Extensions;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using Ejercicio.Persistencia.Metadata;
    using Ejercicio.Trazas;
    using Newtonsoft.Json;

    public class RepositorioGenerico<T> : Repositorio, IRepositorioGenerico<T>, IRepositorio, IDisposable
       where T : class
    {
        private string aliasTabla { get; set; }
        private string nombrePropiedadId { get; set; }
        private string nombrePropiedadUid { get; set; }
        private string nombreTabla { get; set; }
        private PropertyInfo propertyInfoId { get; set; }
        private PropertyInfo propiedadInfoUid { get; set; }
        private List<PropertyInfo> propiedadesTablaInfoInsert { get; set; }
        private List<PropertyInfo> propiedadesTablaInfoUpdate { get; set; }
        private List<string> propiedadesTablaInsert { get; set; }
        private List<string> propiedadesTablaUpdate { get; set; }
        private List<PropertyInfo> propiedadesTablaOneToMany { get; set; }
        private List<PropertyInfo> propiedadesTablaOneToOne { get; set; }
        private List<string> propiedadesRequeridasInsert { get; set; }
        private List<string> propiedadesRequeridasUpdate { get; set; }
        private Dictionary<string, int> propiedadesMaxLength { get; set; }
        private bool heredaDeAuditoria { get; set; }
        private bool softDelete { get; set; }
        internal RepositorioGenerico(string nombrePropiedadId, string nombreTabla, string aliasTabla, List<PropertyInfo> propiedadesTablaInfo, PropertyInfo propertyInfoId, List<PropertyInfo> propiedadesTablaOneToOne, List<PropertyInfo> propiedadesTablaOneToMany, IDatabaseConnectionFactory databaseConnectionFactory, TrazaLoggerInterceptor trazaLoggerInterceptor, bool heredaAuditoria, bool softDelete, List<string> propiedadesRequeridas, List<string> propiedadesRequeridasSoloInsert, Dictionary<string, int> propiedadesMaxLength, string nombrePropiedadUid = null, PropertyInfo propiedadInfoUid = null)
            : base(databaseConnectionFactory, trazaLoggerInterceptor)
        {
            this.propiedadInfoUid = propiedadInfoUid;
            this.nombrePropiedadUid = nombrePropiedadUid;
            this.nombrePropiedadId = nombrePropiedadId;
            this.nombreTabla = nombreTabla;
            this.aliasTabla = aliasTabla;
            this.propiedadesTablaInfoInsert = propiedadesTablaInfo;
            this.propiedadesTablaInsert = propiedadesTablaInfo.Select(p => p.Name).ToList();
            this.propiedadesTablaUpdate = this.propiedadesTablaInsert.Where(p => p != nombrePropiedadId && p != nombrePropiedadUid).ToList();
            this.propiedadesTablaInfoUpdate = propiedadesTablaInfo.Where(p => p.Name != nombrePropiedadId && p.Name != nombrePropiedadUid).ToList();
            this.propertyInfoId = propertyInfoId;
            this.propiedadesTablaOneToOne = propiedadesTablaOneToOne;
            this.propiedadesTablaOneToMany = propiedadesTablaOneToMany;
            this.heredaDeAuditoria = heredaAuditoria;
            this.softDelete = softDelete;
            this.propiedadesRequeridasInsert = propiedadesRequeridas;
            this.propiedadesRequeridasUpdate = propiedadesRequeridas.Where(pr => !propiedadesRequeridasSoloInsert.Contains(pr)).ToList();
            this.propiedadesMaxLength = propiedadesMaxLength;
        }

        public int Count()
        {
            string query = string.Format(ConsultasSQL.SQL_COUNT, this.nombreTabla);
            int total = -1;
            var connection = this.GetConnection();

            try
            {
                total = connection.QuerySingle<int>(query);
            }
            catch (Exception)
            {
            }
            finally
            {
                DisposeConnection(connection);
            }
            DisposeConnection(connection);
            return total;
        }

        #region Obtener
        public IEnumerable<T> GetAll(int take, out int totalElementos, int skip = 0, string orderBy = "", string whereSql = "", IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            var result = this.GetAllCustom<T>(take, out totalElementos, skip, null, "", whereSql, orderBy, transaction, mostrarBorrados);
            return result;
        }

        public IEnumerable<E> GetAllCustom<E>(int take, out int totalElementos, int skip = 0, string propSelectSql = null, string joinsSql = "", string whereSql = "", string orderBy = "", IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            string selectAll = string.Format(ConsultasSQL.SQL_ALL_PROP, this.aliasTabla);
            if (string.IsNullOrEmpty(propSelectSql))
            {
                propSelectSql = selectAll;
            }

            //string propSelectSql = propiedadesSelect.ToPropSelectSql(selectAll);
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = string.Format(ConsultasSQL.SQL_PROP, this.aliasTabla, this.nombrePropiedadId);
            }

            if (!mostrarBorrados && softDelete)
            {

                var filtro = ConsultasSQL.SQL_FILTRO_WHERE_FECHA_BORRADO_ALIAS;

                if (!string.IsNullOrEmpty(whereSql))
                {
                    filtro = ConsultasSQL.SQL_FILTRO_FECHA_BORRADO_ALIAS;

                    whereSql = whereSql.Trim();
                    whereSql = "where (" + whereSql.Substring(5) + " )";

                }

                whereSql += string.Format(filtro, this.aliasTabla);
            }

            string querySelect = string.Format(ConsultasSQL.SQL_GET_ALL_CUSTOM, propSelectSql, this.nombreTabla, this.aliasTabla, joinsSql, whereSql, orderBy, skip, take);
            string queryCount = string.Format(ConsultasSQL.SQL_GET_ALL_CUSTOM_COUNT, propSelectSql, this.nombreTabla, this.aliasTabla, joinsSql, whereSql);

            var query = querySelect + "; " + queryCount;

            return QueryCount<E>(query, out totalElementos, transaction);
        }
        
        public T GetById(object id, IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            T entidad = null;
            if (id == null || id.GetType() != this.propertyInfoId.PropertyType)
            {
                transaction?.RollbackAndCloseConnection();
                //this.RolBackTransactionAndCloseConnection(transaction);
                throw new ArgumentException("Tipo PK invalido");
            }
            string query = string.Format(ConsultasSQL.SQL_GET_BY_ID, this.nombreTabla, this.nombrePropiedadId);

            if (!mostrarBorrados && softDelete)
            {
                query += ConsultasSQL.SQL_FILTRO_FECHA_BORRADO;
            }

            var connection = this.GetConnection(transaction);

            try
            {
                entidad = connection.QuerySingleOrDefault<T>(query, new { ID = id }, transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "GetById",
                    ex,
                    new List<string>() { query, id.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return entidad;
        }

        public T GetByPK(IPrimaryKey primaryKey, IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            T entidad = null;
            if (primaryKey == null)
            {
                transaction?.RollbackAndCloseConnection();
                //this.RolBackTransactionAndCloseConnection(transaction);
                throw new ArgumentException("Tipo PK invalido");
            }
            string query = string.Format(ConsultasSQL.SQL_GET_BY_KEY, this.nombreTabla, primaryKey.Where);

            if (!mostrarBorrados && softDelete)
            {
                query += ConsultasSQL.SQL_FILTRO_FECHA_BORRADO;
            }

            var connection = this.GetConnection(transaction);

            try
            {
                entidad = connection.QuerySingleOrDefault<T>(query, primaryKey, transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "GetById",
                    ex,
                    new List<string>() { query, primaryKey.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return entidad;
        }

        public T GetByUid(Guid Uid, IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            T entidad = null;
            if (nombrePropiedadUid == null)
            {
                return entidad;
            }
            string query = string.Format(ConsultasSQL.SQL_GET_BY_Uid, this.nombreTabla, nombrePropiedadUid);
            if (!mostrarBorrados && softDelete)
            {
                query += ConsultasSQL.SQL_FILTRO_FECHA_BORRADO;
            }
            var connection = this.GetConnection(transaction);

            try
            {
                entidad = connection.QuerySingleOrDefault<T>(query, new { Uid = Uid }, transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "GetById",
                    ex,
                    new List<string>() { query, Uid.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return entidad;
        }

        public List<T> GetByIds(List<object> ids, IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            List<T> entidad = new List<T>();
            if (ids == null || !ids.Any())
            {
                return new List<T>();
            }

            if (ids.First().GetType() != this.propertyInfoId.PropertyType)
            {
                transaction?.RollbackAndCloseConnection();
                //this.RolBackTransactionAndCloseConnection(transaction);
                throw new ArgumentException("Tipo PK invalido");
            }

            string query = string.Format(ConsultasSQL.SQL_GET_BY_IDS, this.nombreTabla, this.nombrePropiedadId);

            if (!mostrarBorrados && softDelete)
            {
                query += ConsultasSQL.SQL_FILTRO_FECHA_BORRADO;
            }

            var connection = this.GetConnection(transaction);

            try
            {
                entidad = connection.Query<T>(query, new { IDs = ids }, transaction).ToList();
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "GetByIds",
                    ex,
                    new List<string>() { query, string.Join(", ", ids.Select(i => i.ToString())) }
                    );
                transaction?.RollbackAndCloseConnection();
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return entidad;
        }

        #endregion

        #region Insertar
        public bool Insert(T entidad, IDbTransaction transaction = null)
        {
            string propiedades = string.Empty;
            string propiedadesInsert = string.Empty;

            if (entidad.GetType().GetInterfaces().Contains(typeof(IPrimaryKey)))
            {
                IEnumerable<string> listaConcat = ((IPrimaryKey)entidad).ListaParametros.Concat(this.propiedadesTablaInsert);
                propiedades = string.Join(",", listaConcat.Select(elemento => $"[{elemento}]"));
                propiedadesInsert = string.Join(",", listaConcat.Select(p => string.Format(ConsultasSQL.SQL_PARAMETRO, p)));
            }
            else {
                propiedades = string.Join(",", this.propiedadesTablaInsert.Select(elemento => $"[{elemento}]"));
                propiedadesInsert = string.Join(",", propiedadesTablaInsert.Select(p => string.Format(ConsultasSQL.SQL_PARAMETRO, p)));
            }

            return Insert(entidad, transaction, propiedadesTablaInfoInsert, propiedades, propiedadesInsert, propertyInfoId, propiedadInfoUid);
        }
        public bool Insert<E>(E entidad, IDbTransaction transaction = null)
        {
            var tipoEntidad = typeof(E);
            var propiedadesE = tipoEntidad.GetProperties().Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null);
            var proInfoId = propiedadesE.Where(p => p.Name == this.nombrePropiedadId).FirstOrDefault();
            var proInfoUid = propiedadesE.Where(p => p.Name == this.nombrePropiedadUid).FirstOrDefault();
            var propiedadesEntidad = propiedadesE.Select(p => p.Name).ToList();


            var propiedadesTablaInsertFiltradas = propiedadesTablaInsert.Where(p => propiedadesEntidad.Contains(p));

            var propiedades = string.Join(",", propiedadesTablaInsertFiltradas.Select(elemento => $"[{elemento}]"));
            var propiedadesInsert = string.Join(",", propiedadesTablaInsertFiltradas.Select(p => string.Format(ConsultasSQL.SQL_PARAMETRO, p)));

            return Insert<E>(entidad, transaction, propiedadesE.ToList(), propiedades, propiedadesInsert, proInfoId, proInfoUid);
        }

        private bool Insert<E>(E entidad, IDbTransaction transaction, List<PropertyInfo> propiedadesInsertInfo, string propiedades, string propiedadesInsert, PropertyInfo proInfoId = null, PropertyInfo proInfoUid = null)
        {
            if (entidad == null)
            {
                return false;
            }
            if (heredaDeAuditoria)
            {
                propiedades += ConsultasSQL.SQL_PROPIEDADES_AUDITORIA_INSERT;
                propiedadesInsert += string.Format(ConsultasSQL.SQL_PROPIEDADES_AUDITORIA_INSERT_VALUE, UsuarioContexto.Nombre);
            }

            var query = string.Format(ConsultasSQL.SQL_INSERT_INTO, this.nombreTabla, propiedades, propiedadesInsert);
            Guid UidNuevo = Guid.Empty;
            if (nombrePropiedadUid != null)
            {
                var valor = proInfoUid.GetValue(entidad);
                if (valor != null)
                {
                    UidNuevo = (Guid)valor;
                }

                if (UidNuevo == null || UidNuevo == Guid.Empty)
                {
                    UidNuevo = Guid.NewGuid();
                    proInfoUid.SetValue(entidad, UidNuevo);
                }

                /* if (UidNuevo == null || UidNuevo == Guid.Empty)
                 {
                     UidNuevo = Guid.NewGuid();
                     proInfoUid.SetValue(entidad, UidNuevo);
                 }
                 else
                 {*/
                var getId = string.Format(ConsultasSQL.SQL_INSERT_INTO_GET_ID_BY_Uid, nombrePropiedadId, nombreTabla, nombrePropiedadUid, UidNuevo);
                query = string.Format(ConsultasSQL.SQL_INSERT_CHECK_Uid, this.nombreTabla, this.nombrePropiedadUid, UidNuevo.ToString(), query, getId);
                //}
            }
            else
            {
                query += ConsultasSQL.SQL_INSERT_INTO_GET_ID_BY_SCOPE;
            }

            var connection = this.GetConnection(transaction);
            var respuesta = false;

            try
            {
                ValidarInsert(entidad, propiedadesInsertInfo);

                var respuestaId = /*new {Id = 1};//*/ connection.Query(query, param: entidad, transaction: transaction).SingleOrDefault();
                if (proInfoId != null)
                {
                    proInfoId.SetValue(entidad, Convert.ChangeType(respuestaId.Id, proInfoId.PropertyType), null);
                }
                respuesta = respuestaId.Id != 0;
            }
            catch (ValidationDBException vex)
            {
                throw vex;
            }
            catch (Exception ex)
            {
                respuesta = false;
                if (nombrePropiedadUid != null)
                {
                    proInfoUid.SetValue(entidad, UidNuevo);
                }

                transaction?.RollbackAndCloseConnection();
                var errorValidacion = ValidationDBException.Parse(ex);
                if (errorValidacion != null)
                {
                    throw errorValidacion;
                }
                else
                {
                    var param = entidad == null ? "[Entidad = null]" : JsonConvert.SerializeObject(entidad);
                    trazaLoggerInterceptor.GuardarExcepcion(
                        Guid.NewGuid(),
                        UsuarioContexto.Nombre,
                        "Db",
                        "Ejercicio.Service",
                        "Ejercicio.Service",
                        "Insert",
                        ex,
                        new List<string>() { query, param }
                        );
                }
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return respuesta;
        }
        #endregion

        #region Actualizar
        public bool Update(T entidad, IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            var propiedadesSet = string.Join(",", propiedadesTablaUpdate.Select(p => string.Format(ConsultasSQL.SQL_SET, p)));

            return Update<T>(entidad, transaction, propiedadesTablaInfoUpdate, propiedadesSet, mostrarBorrados);
        }
        public bool Update<E>(E entidad, IDbTransaction transaction = null, bool mostrarBorrados = false)
        {
            var tipoEntidad = typeof(E);
            var propiedadesEntidad = tipoEntidad.GetProperties().Where(p => p.GetCustomAttribute<NotMappedAttribute>() == null);
            propiedadesEntidad = propiedadesEntidad.Where(pe => propiedadesTablaUpdate.Contains(pe.Name));


            var propiedadesSet = string.Join(",", propiedadesEntidad.Select(pe => string.Format(ConsultasSQL.SQL_SET, pe.Name)));

            return Update<E>(entidad, transaction, propiedadesEntidad.ToList(), propiedadesSet, mostrarBorrados);
        }

        private bool Update<E>(E entidad, IDbTransaction transaction, List<PropertyInfo> propiedadesInsertInfo, string propiedadesSet, bool mostrarBorrados = false)
        {
            if (entidad == null)
            {
                return false;
            }

            if (heredaDeAuditoria)
            {
                propiedadesSet += string.Format(ConsultasSQL.SQL_PROPIEDADES_AUDITORIA_UPDATE, UsuarioContexto.Nombre);
            }
            int entidadesActualizadas;
            string query = string.Empty;

            if (entidad.GetType().GetInterfaces().Contains(typeof(IPrimaryKey))) {
                query = string.Format(ConsultasSQL.SQL_UPDATE_PK, this.nombreTabla, propiedadesSet, ((IPrimaryKey)entidad).Where);
            } else
                query = string.Format(ConsultasSQL.SQL_UPDATE, this.nombreTabla, propiedadesSet, this.nombrePropiedadId);

            if (!mostrarBorrados && softDelete)
            {
                query += ConsultasSQL.SQL_FILTRO_FECHA_BORRADO;
            }

            var connection = this.GetConnection(transaction);
            
            try
            {
                ValidarUpdate(entidad, propiedadesInsertInfo);
                entidadesActualizadas = connection.Execute(query, param: entidad, transaction: transaction);
            }
            catch (ValidationDBException vex)
            {
                throw vex;
            }
            catch (Exception ex)
            {
                transaction?.RollbackAndCloseConnection();
                var errorValidacion = ValidationDBException.Parse(ex);
                if (errorValidacion != null)
                {
                    throw errorValidacion;
                }
                else
                {
                    var param = entidad == null ? "[Entidad = null]" : JsonConvert.SerializeObject(entidad);
                    trazaLoggerInterceptor.GuardarExcepcion(
                        Guid.NewGuid(),
                        UsuarioContexto.Nombre,
                        "Db",
                        "Ejercicio.Service",
                        "Ejercicio.Service",
                        "Update",
                        ex,
                        new List<string>() { query, param }
                        );
                }
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return entidadesActualizadas > 0;
        }
        #endregion

        #region Eliminar
        public bool SoftDeleteAllowed()
        {
            return softDelete;
        }

        public bool SoftDelete(IPrimaryKey primarykey, IDbTransaction transaction = null) { 
            if (!softDelete)
            {
                return false;
            }

            int elementosEliminados;

            string query; // = string.Empty;

            if (heredaDeAuditoria)
            {
                query = string.Format(ConsultasSQL.SQL_SOFT_DELETE_BY_PK_AUDITORIA, this.nombreTabla, UsuarioContexto.Nombre, primarykey.Where);
            }
            else
            {
                query = string.Format(ConsultasSQL.SQL_SOFT_DELETE_BY_PK, this.nombreTabla, primarykey.Where);
            }

            var connection = this.GetConnection(transaction);

            try
            {
                elementosEliminados = connection.Execute(query, primarykey);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "SoftDelete",
                    ex,
                    new List<string>() { query, primarykey.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            return elementosEliminados > 0;
        }

        public bool SoftDelete(int id, IDbTransaction transaction = null)
        {
            if (!softDelete)
            {
                return false;
            }

            int elementosEliminados;

            string query; // = string.Empty;

            if (heredaDeAuditoria)
            {
                query = string.Format(ConsultasSQL.SQL_SOFT_DELETE_BY_Uid_AUDITORIA, this.nombreTabla, UsuarioContexto.Nombre, this.nombrePropiedadId);
            }
            else
            {
                query = string.Format(ConsultasSQL.SQL_SOFT_DELETE_BY_Uid, this.nombreTabla, this.nombrePropiedadId);
            }

            var connection = this.GetConnection(transaction);

            try
            {
                elementosEliminados = connection.Execute(query, new { Uid = id });
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "SoftDelete",
                    ex,
                    new List<string>() { query, id.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            return elementosEliminados > 0;
        }
        public bool SoftDelete(Guid Uid, IDbTransaction transaction = null)
        {
            if (!softDelete)
            {
                return false;
            }

            int elementosEliminados;

            string query; // = string.Empty;

            if (heredaDeAuditoria)
            {
                query = string.Format(ConsultasSQL.SQL_SOFT_DELETE_BY_Uid_AUDITORIA, this.nombreTabla, UsuarioContexto.Nombre, this.nombrePropiedadUid);
            }
            else
            {
                query = string.Format(ConsultasSQL.SQL_SOFT_DELETE_BY_Uid, this.nombreTabla, this.nombrePropiedadUid);
            }

            var connection = this.GetConnection(transaction);

            try
            {
                elementosEliminados = connection.Execute(query, new { Uid = Uid });
            }
            catch (Exception)
            {
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            return elementosEliminados > 0;
        }

        public bool UndoSoftDelete(int id, IDbTransaction transaction = null)
        {
            if (!softDelete)
            {
                return false;
            }

            int elementosRecuperados;

            string query; // = string.Empty;

            if (heredaDeAuditoria)
            {
                query = string.Format(ConsultasSQL.SQL_UNDO_SOFT_DELETE_BY_Uid_AUDITORIA, this.nombreTabla, UsuarioContexto.Nombre, this.nombrePropiedadId);
            }
            else
            {
                query = string.Format(ConsultasSQL.SQL_UNDO_SOFT_DELETE_BY_Uid, this.nombreTabla, this.nombrePropiedadId);
            }

            var connection = this.GetConnection(transaction);

            try
            {
                elementosRecuperados = connection.Execute(query, new { Uid = id });
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "UndoSoftDelete",
                    ex,
                    new List<string>() { query, id.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            return elementosRecuperados > 0;
        }

        public bool UndoSoftDelete(Guid Uid, IDbTransaction transaction = null)
        {
            if (!softDelete)
            {
                return false;
            }

            int elementosRecuperados;
            string query; // = string.Empty;

            if (heredaDeAuditoria)
            {
                query = string.Format(ConsultasSQL.SQL_UNDO_SOFT_DELETE_BY_Uid_AUDITORIA, this.nombreTabla, UsuarioContexto.Nombre, this.nombrePropiedadUid);
            }
            else
            {
                query = string.Format(ConsultasSQL.SQL_UNDO_SOFT_DELETE_BY_Uid, this.nombreTabla, this.nombrePropiedadUid);
            }

            var connection = this.GetConnection(transaction);

            try
            {
                elementosRecuperados = connection.Execute(query, new { Uid = Uid });
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "UndoSoftDelete",
                    ex,
                    new List<string>() { query, Uid.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            return elementosRecuperados > 0;
        }
        public bool DeleteByPK(IPrimaryKey primaryKey, IDbTransaction transaction = null)
        {
            int elementosEliminados;
            if (primaryKey == null)
            {
                transaction?.RollbackAndCloseConnection();
                throw new ArgumentException("Tipo PK invalido");
            }
            string query = string.Format(ConsultasSQL.SQL_DELETE_BY_PK, this.nombreTabla, primaryKey.Where);
            var connection = this.GetConnection(transaction);

            try
            {
                elementosEliminados = connection.Execute(query, primaryKey, transaction: transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "Delete",
                    ex,
                    new List<string>() { query, primaryKey.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return elementosEliminados == 1;
        }

        public bool Delete(object id, IDbTransaction transaction = null)
        {
            int elementosEliminados;
            if (id == null || id.GetType() != this.propertyInfoId.PropertyType)
            {
                transaction?.RollbackAndCloseConnection();
                throw new ArgumentException("Tipo PK invalido");
            }
            string query = string.Format(ConsultasSQL.SQL_DELETE_BY_ID, this.nombreTabla, this.nombrePropiedadId);
            var connection = this.GetConnection(transaction);

            try
            {
                elementosEliminados = connection.Execute(query, new { ID = id }, transaction: transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "Delete",
                    ex,
                    new List<string>() { query, id.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return elementosEliminados == 1;
        }
        public bool DeleteByUid(Guid Uid, IDbTransaction transaction = null)
        {
            if (nombrePropiedadUid == null)
            {
                return false;
            }
            int elementosEliminados;
            string query = string.Format(ConsultasSQL.SQL_DELETE_BY_Uid, this.nombreTabla, this.nombrePropiedadUid);
            var connection = this.GetConnection(transaction);

            try
            {
                elementosEliminados = connection.Execute(query, new { Uid = Uid }, transaction: transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Service",
                    "Ejercicio.Service",
                    "Delete",
                    ex,
                    new List<string>() { query, Uid.ToString() }
                    );
                transaction?.RollbackAndCloseConnection();
                return false;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }

            return elementosEliminados >= 1;
        }
        #endregion

        #region Validaciones
        private void ValidarInsert<E>(E entidad, List<PropertyInfo> propiedadesInfo)
        {
            Validar(entidad, propiedadesInfo, false);
        }

        private void ValidarUpdate<E>(E entidad, List<PropertyInfo> propiedadesInfo)
        {
            Validar(entidad, propiedadesInfo, true);
        }

        private void Validar<E>(E entidad, List<PropertyInfo> propiedadesInfo, bool operacionUpdate)
        {
            var listaErrores = new List<string>();
            ValidarPropiedadesRequeridas(entidad, propiedadesInfo, listaErrores, operacionUpdate);

            ValidarPropiedadesMaxLength(entidad, propiedadesInfo, listaErrores);

            if (listaErrores.Any())
            {
                throw new ValidationDBException(listaErrores);
            }
        }

        private void ValidarPropiedadesRequeridas<E>(E entidad, List<PropertyInfo> propiedadesInfo, List<string> listaErrores, bool operacionUpdate)
        {
            (operacionUpdate ? propiedadesRequeridasUpdate : propiedadesRequeridasInsert).ForEach(pr =>
            {
                var propertyInfo = propiedadesInfo.FirstOrDefault(p => p.Name == pr);
                if (propertyInfo == null)
                {
                    if (!operacionUpdate)
                    {
                        listaErrores.Add("La propiedad " + pr + " es requerida.");
                    }
                }
                else
                {
                    var valor = propertyInfo.GetValue(entidad);
                    if (valor == null)
                    {
                        listaErrores.Add("La propiedad " + propertyInfo.Name.SpacesFromCamel() + " es requerida.");
                    }
                    else if (propertyInfo.PropertyType.Name == "Int32"
                            && propertyInfo.Name.ToLower().EndsWith("id")
                            && ((int)valor == 0))
                    {
                        var nombreFK = propertyInfo.Name.Substring(0, propertyInfo.Name.Length - 2);
                        listaErrores.Add("La fk " + nombreFK.SpacesFromCamel() + " es requerida.");
                    }
                }
            });
        }

        private void ValidarPropiedadesMaxLength<E>(E entidad, List<PropertyInfo> propiedadesInfo, List<string> listaErrores)
        {
            propiedadesMaxLength.Keys.ToList().ForEach(pr =>
            {
                var propertyInfo = propiedadesInfo.FirstOrDefault(p => p.Name == pr);
                if (propertyInfo != null)
                {
                    var valor = propertyInfo.GetValue(entidad);
                    var maxLen = propiedadesMaxLength[pr];
                    if (valor != null && valor.ToString().Length > maxLen)
                    {
                        listaErrores.Add("La propiedad " + propertyInfo.Name.SpacesFromCamel() + " tiene una longitud máxima de " + maxLen);
                    }
                }
            });
        }

        #endregion

        #region Patron Disposable

        private bool disposed = false;

        protected new virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose-only, i.e. non-finalizable logic
                    base.Dispose();       
                }

                // shared cleanup logic
                disposed = true;
            }
        }

        ~RepositorioGenerico()
        {
            Dispose(false);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
