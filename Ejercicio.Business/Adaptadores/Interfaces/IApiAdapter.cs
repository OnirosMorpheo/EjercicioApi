
namespace Ejercicio.Business.Adaptadores
{
    public interface IApiAdapter<DtoGet, DtoPost, Model> : IAdapter<DtoGet, Model> where DtoGet : class, new() where DtoPost : class, new() where Model : class, new()
    {
        DtoPost ToDtoPost(Model obj);
    }
}
