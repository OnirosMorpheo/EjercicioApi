

namespace Ejercicio.Persistencia.Interfaces
{

    public interface IRepositorioGenericoFactory
    {
        IRepositorioGenerico<T> GetRepositorio<T>() where T : class;
    }
}
