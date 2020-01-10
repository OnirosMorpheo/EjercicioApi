

namespace Ejercicio.Persistencia
{
    using Ejercicio.Persistencia.Interfaces;
    using Ejercicio.Persistencia.Metadata;
    using Ejercicio.Trazas;
    using Ejercicio.Utilities;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    public class Repositorio : IRepositorio, IDisposable
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }
        protected TrazaLoggerInterceptor trazaLoggerInterceptor;

        public Repositorio(IDatabaseConnectionFactory databaseConnectionFactory, TrazaLoggerInterceptor trazaLoggerInterceptor)
        {
            this.DatabaseConnectionFactory = databaseConnectionFactory;
            this.trazaLoggerInterceptor = trazaLoggerInterceptor;
        }

        internal IDbConnection GetConnection(IDbTransaction transaction = null)
        {
            IDbConnection connection;
            if (transaction != null)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "DB",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "GetConnection",
                    new Exception("GetConnection -> IDbTransaction != null. La funcionalidad de las transacciones esta deshabilitada"));
            }
            /*Debug.WriteLine("GetConnection transaction " + (transaction != null).ToString());

            if (transaction != null)
            {
                connection = transaction.Connection;
                if (connection.State != ConnectionState.Open)
                {
                    trazaLoggerInterceptor.GuardarExcepcion(Guid.NewGuid(), UsuarioContexto.Nombre, "GetConnection connection.State != ConnectionState.Open", "", "", "", new Exception(Environment.StackTrace));
                    connection.Open();
                }
            }
            else
            {*/
            connection = this.DatabaseConnectionFactory.GetNewConnection();
            connection.Open();
            /*}
            Debug.WriteLine("GetConnection transaction " + (transaction != null).ToString() + ". Connection:" + connection.State);
            */
            return connection;
        }

        public IEnumerable<E> Query<E>(string query, IDbTransaction transaction = null)
        {
            IEnumerable<E> result = null;
            var connection = this.GetConnection(transaction);
            try
            {
                result = connection.Query<E>(query, transaction: transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "Query<E>",
                    ex,
                    new List<string>() { query }
                    );
                throw ex;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            DisposeConnection(connection, transaction);
            return result;
        }

        public IEnumerable<E> QueryCount<E>(string query, out int totalElementos, IDbTransaction transaction = null)
        {
            IEnumerable<E> result = null;
            totalElementos = -1;
            var connection = this.GetConnection(transaction);

            try
            {
                using var multi = connection.QueryMultiple(query, transaction: transaction);
                result = multi.Read<E>().ToList();
                totalElementos = multi.Read<int>().FirstOrDefault();
            }
            catch (Exception ex)
            {

                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "QueryCount<E>",
                    ex,
                    new List<string>() { query }
                    );
                throw ex;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            DisposeConnection(connection, transaction);
            return result;
        }

        internal static void DisposeConnection(IDbConnection connection, IDbTransaction transaction = null)
        {
            Debug.WriteLine("DisposeConnection " + connection.State + ". transaction " + (transaction != null).ToString());
            if (transaction == null)
            {
                connection.Close();
                connection.Dispose();
                Debug.WriteLine("DisposeConnection " + connection.State);
            }
        }

        public List<List<E>> QueryMultiple<E>(List<string> queries)
        {
            List<List<E>> result = new List<List<E>>();
            var connection = this.GetConnection();
            try
            {
                var sql = string.Join("; ", queries);
                using (var multi = connection.QueryMultiple(sql))
                {
                    queries.ForEach(q =>
                    {
                        result.Add(multi.Read<E>().ToList());
                    });
                }
            }
            catch (Exception ex)
            {

                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "QueryMultiple<E>",
                    ex,
                    queries
                    );
                throw ex;
            }
            finally
            {
                DisposeConnection(connection);
            }
            DisposeConnection(connection);
            return result;
        }

        public List<int?> ResolverUids(List<Tuple<Type, EntidadClaveExterna>> Uids)
        {
            if (Uids == null || !Uids.Any())
            {
                return new List<int?>();
            }

            var queries = Uids.Select(t => t.Item2 != null && t.Item2.Uid.HasValue
                            ? string.Format(ConsultasSQL.SQL_RESOLVER_Uid, t.Item1.NombreTabla(), t.Item2.Uid)
                            : ConsultasSQL.SQL_RESOLVER_Uid_NULL)
            .ToList();
            var resultado = QueryMultiple<int?>(queries);
            return resultado.Select(l => l.SingleOrDefault()).ToList();
        }

        public List<EntidadClaveExterna> SecurizarFKs(List<Tuple<Type, int?>> Uids)
        {
            if (Uids == null || !Uids.Any())
            {
                return new List<EntidadClaveExterna>();
            }

            var queries = Uids.Select(t => t.Item2.HasValue
                        ? string.Format(ConsultasSQL.SQL_SECURIZAR_ID, t.Item1.PropiedadDescripcionTabla(), t.Item1.NombreTabla(), t.Item2)
                        : ConsultasSQL.SQL_SECURIZAR_ID_NULL)
            .ToList();
            var resultado = QueryMultiple<EntidadClaveExterna>(queries);
            return resultado.Select(l => l.SingleOrDefault()).ToList();
        }

        public IEnumerable<E> QueryFromTableValued<E>(ITableValued<E> tableValued, IDbTransaction transaction = null) {
            IEnumerable<E> result = null;
            var connection = this.GetConnection(transaction);

            string query = tableValued.Query;

            try
            {
                result = connection.Query<E>(query, param: tableValued, transaction: transaction);
            }
            catch (Exception ex)
            {
                trazaLoggerInterceptor.GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Nombre,
                    "Db",
                    "Ejercicio.Infraestructura.DB",
                    "Ejercicio.Infraestructura.DB",
                    "QueryFromTableValued<E>",
                    ex,
                    new List<string>() { query }
                    );
                throw ex;
            }
            finally
            {
                DisposeConnection(connection, transaction);
            }
            DisposeConnection(connection, transaction);
            return result;
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

        ~Repositorio()
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
