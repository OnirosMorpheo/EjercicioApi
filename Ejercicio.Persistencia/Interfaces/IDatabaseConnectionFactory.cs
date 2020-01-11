
namespace Ejercicio.Persistence.Interfaces
{
    using System.Data;

    public interface IDatabaseConnectionFactory
    {
        IDbConnection GetNewConnection();
    }
}
