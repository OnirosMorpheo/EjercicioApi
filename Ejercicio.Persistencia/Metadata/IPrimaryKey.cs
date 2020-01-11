

namespace Ejercicio.Persistence.Metadata
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    public interface IPrimaryKey
    {
        [NotMapped]
        int Count { get; }
        [NotMapped]
        string Where { get; }
        [NotMapped]
        string Select { get; }
        [NotMapped]
        List<string> ListaParametros { get; }
    }
}
