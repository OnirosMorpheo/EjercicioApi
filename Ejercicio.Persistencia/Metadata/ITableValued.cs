

namespace Ejercicio.Persistencia.Metadata
{
    using System.Collections.Generic;
    public interface ITableValued<T>
    {
        public string Query { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
