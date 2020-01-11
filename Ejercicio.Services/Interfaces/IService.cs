
namespace Ejercicio.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IService<TModel, TKey> where TModel : class, new()
    {
        Task<bool> DeleteAsync(TKey id);
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetAsync(TKey id);
        Task<bool> ExistsAsync(TKey id);
        Task<TModel> GetAsync(TModel entidad);
        Task<bool> Save(TModel entidad);
    }
}
