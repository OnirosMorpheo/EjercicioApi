
namespace Ejercicio.Persistencia.Interfaces
{
    using System.Data;

    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetNewConnection();
    }
}
