
namespace Ejercicio.Persistence
{
    using Ejercicio.Persistence.Interfaces;
    using Ejercicio.Persistence.Metadata;
    using Ejercicio.Trazas;
    using Ejercicio.Utilities;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    public abstract class Procedure<T, Q> : IProcedure<Q>, IDisposable where T : class, IParamsProcedure, new()
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }
        
        protected TrazaLoggerInterceptor trazaLoggerInterceptor;
        private Dictionary<Type, DbType> typeMap;

        public virtual string Name { get; set; }
        public virtual IEnumerable<Q> Results { get; set; }

        public Procedure(IDatabaseConnectionFactory databaseConnectionFactory, TrazaLoggerInterceptor trazaLoggerInterceptor)
        {
            this.DatabaseConnectionFactory = databaseConnectionFactory;
            this.trazaLoggerInterceptor = trazaLoggerInterceptor;
            InicializarTipos();
        }

        private void InicializarTipos() {
            typeMap = new Dictionary<Type, DbType>();
            typeMap[typeof(byte)] = DbType.Byte;
            typeMap[typeof(sbyte)] = DbType.SByte;
            typeMap[typeof(short)] = DbType.Int16;
            typeMap[typeof(ushort)] = DbType.UInt16;
            typeMap[typeof(int)] = DbType.Int32;
            typeMap[typeof(uint)] = DbType.UInt32;
            typeMap[typeof(long)] = DbType.Int64;
            typeMap[typeof(ulong)] = DbType.UInt64;
            typeMap[typeof(float)] = DbType.Single;
            typeMap[typeof(double)] = DbType.Double;
            typeMap[typeof(decimal)] = DbType.Decimal;
            typeMap[typeof(bool)] = DbType.Boolean;
            typeMap[typeof(string)] = DbType.String;
            typeMap[typeof(char)] = DbType.StringFixedLength;
            typeMap[typeof(Guid)] = DbType.Guid;
            typeMap[typeof(DateTime)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset)] = DbType.DateTimeOffset;
            typeMap[typeof(byte[])] = DbType.Binary;
            typeMap[typeof(byte?)] = DbType.Byte;
            typeMap[typeof(sbyte?)] = DbType.SByte;
            typeMap[typeof(short?)] = DbType.Int16;
            typeMap[typeof(ushort?)] = DbType.UInt16;
            typeMap[typeof(int?)] = DbType.Int32;
            typeMap[typeof(uint?)] = DbType.UInt32;
            typeMap[typeof(long?)] = DbType.Int64;
            typeMap[typeof(ulong?)] = DbType.UInt64;
            typeMap[typeof(float?)] = DbType.Single;
            typeMap[typeof(double?)] = DbType.Double;
            typeMap[typeof(decimal?)] = DbType.Decimal;
            typeMap[typeof(bool?)] = DbType.Boolean;
            typeMap[typeof(char?)] = DbType.StringFixedLength;
            typeMap[typeof(Guid?)] = DbType.Guid;
            typeMap[typeof(DateTime?)] = DbType.DateTime;
            typeMap[typeof(DateTimeOffset?)] = DbType.DateTimeOffset;
            typeMap[typeof(System.Data.Linq.Binary)] = DbType.Binary;
        }

        internal IDbConnection GetConnection()
        {
            IDbConnection connection;

            connection = this.DatabaseConnectionFactory.GetNewConnection();
            connection.Open();

            return connection;
        }

        internal static void DisposeConnection(IDbConnection connection)
        {
            Debug.WriteLine($"DisposeConnection {connection.State}.");
            connection.Close();
            connection.Dispose();
            Debug.WriteLine($"DisposeConnection {connection.State}");
        }

        public virtual IEnumerable<Q> Execute(T parametros)
        {
            var connection = this.GetConnection();
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                Type tipo = parametros.GetType();
                Dictionary<string, PropertyInfo> propiedades = tipo.GetProperties().ToDictionary(elemento => $"@{elemento.Name}", elemento => elemento);
                foreach (var parametro in parametros.Lista)
                {
                    if (propiedades.ContainsKey(parametro))
                        parameter.Add(parametro, propiedades[parametro].GetValue(parametros));
                }
                Results = connection.Query<Q>(this.Name, parameter, commandType:CommandType.StoredProcedure);
                return Results;
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "Procedure<T,Q>",
                    ex,
                    new List<string>() { "" }
                    );
                throw ex;
            }
            finally
            {
                DisposeConnection(connection);
            }
        }

        public virtual Q ExecuteScalar(T parametros)
        {            
            var connection = this.GetConnection();
            try
            {
                DynamicParameters parameter = new DynamicParameters();
                Type tipo = parametros.GetType();
                Dictionary<string, PropertyInfo> propiedades = tipo.GetProperties().ToDictionary(elemento => $"@{elemento.Name}", elemento => elemento);
                foreach (var parametro in parametros.Lista)
                {
                    if (propiedades.ContainsKey(parametro))
                        parameter.Add(parametro, propiedades[parametro].GetValue(parametros) ?? DBNull.Value);
                }
                parameter.Add(name: "@RetVal",
                    dbType: typeMap[typeof(Q)],
                    direction: ParameterDirection.ReturnValue
                    );
                connection.Execute(this.Name, parameter, commandType: CommandType.StoredProcedure);
                this.Results = new List<Q>();

                return parameter.Get<Q>("@RetVal");
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "TableValuedExecute<Q>",
                    ex,
                    new List<string>() { "" }
                    );
                throw ex;
            }
            finally
            {
                DisposeConnection(connection);
            }
        }

        #region Patron Disposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose-only, i.e. non-finalizable logic
                }

                // shared cleanup logic
                disposed = true;
            }
        }

        ~Procedure()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #endregion
    }
}
