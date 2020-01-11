

namespace Ejercicio.Persistence
{
    using Ejercicio.Persistence.Interfaces;
    using Ejercicio.Utilities;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    public class SqlConnectionFactory : IDatabaseConnectionFactory
    {
        public SqlConnectionFactory()
        {
        }

        public IDbConnection GetNewConnection()
        {
            return new SqlConnection(Utils.ConnectionStrings("conexionEjercicio"));
        }

    }
}
