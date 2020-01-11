

namespace Ejercicio.Persistence.Interfaces
{
    public interface IRepositorioGenericoFactory
    {
        IRepositorioGenerico<T> GetRepositorio<T>() where T : class;
    }
}
