

namespace Ejercicio.Persistence
{
    using System;
    using System.Data;
    public static class DbTransactionExtension
    {
        public static void CommitAndCloseConnection(this IDbTransaction transaction)
        {
            if (transaction == null)
                return;
            var conn = transaction.Connection;
            try
            {
                transaction.Commit();
            }
            finally
            {
                transaction.Dispose();
                conn?.Dispose();
            }
        }

        public static void RollbackAndCloseConnection(this IDbTransaction transaction)
        {
            if (transaction == null || transaction.Connection == null || transaction.Connection.State != ConnectionState.Open)
                return;
            var conn = transaction.Connection;
            try
            {
                transaction.Rollback();
            }
            catch (Exception)
            {

            }
            finally
            {
                transaction.Dispose();
                conn?.Dispose();
            }
        }
        public static void DisposeAndCloseConnection(this IDbTransaction transaction)
        {
            if (transaction == null)
                return;
            var conn = transaction.Connection;
            transaction.Dispose();
            conn?.Dispose();
        }
    }
}
