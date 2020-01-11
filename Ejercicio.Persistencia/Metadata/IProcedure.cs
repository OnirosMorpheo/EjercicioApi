

namespace Ejercicio.Persistence.Metadata
{
    using System.Collections.Generic;
    public interface IProcedure<T>
    {
        string Name { get; set; }
        IEnumerable<T> Results { get; set; }
    }
}
