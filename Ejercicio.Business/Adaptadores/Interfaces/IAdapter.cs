
namespace Ejercicio.Business.Adaptadores
{
    using System.Collections.Generic;

    public interface IAdapter<DtoGet, Model> where DtoGet : class, new() where Model : class, new()
    {
        DtoGet ToDto(Model obj);
        IEnumerable<DtoGet> ToIEnumerableDto(IEnumerable<Model> lista);
        Model ToModel(DtoGet obj);
        IEnumerable<Model> ToIEnumerableModel(IEnumerable<DtoGet> lista);
    }
}
