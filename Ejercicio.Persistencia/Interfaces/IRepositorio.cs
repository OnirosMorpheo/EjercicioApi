
namespace Ejercicio.Persistence.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface IRepositorio : IDisposable
    {
        IEnumerable<E> Query<E>(string query, IDbTransaction transaction = null);
        IEnumerable<E> QueryCount<E>(string query, out int totalElementos, IDbTransaction transaction = null);
        List<List<E>> QueryMultiple<E>(List<string> queries);
        List<int?> ResolverUids(List<Tuple<Type, EntidadClaveExterna>> Uids);
        List<EntidadClaveExterna> SecurizarFKs(List<Tuple<Type, int?>> Uids);
    }
}
