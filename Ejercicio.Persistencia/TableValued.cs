

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
    public abstract class TableValued<T, Q> : ITableValued<Q>, IDisposable where T : class, IParamsTableValued, new() where Q : class, new()
    {
        private IDatabaseConnectionFactory DatabaseConnectionFactory { get; set; }
        public virtual string Query { get; set; }
        public virtual IEnumerable<Q> Results { get; set; }

        protected TrazaLoggerInterceptor trazaLoggerInterceptor;
        
        public TableValued(IDatabaseConnectionFactory databaseConnectionFactory, TrazaLoggerInterceptor trazaLoggerInterceptor)
        {
            this.DatabaseConnectionFactory = databaseConnectionFactory;
            this.trazaLoggerInterceptor = trazaLoggerInterceptor;
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
                Results = connection.Query<Q>(Query, parametros);
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
                    "TableValuedExecute<Q>",
                    ex,
                    new List<string>() { parametros.Lista }
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

        ~TableValued()
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
