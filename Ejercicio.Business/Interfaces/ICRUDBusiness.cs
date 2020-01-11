
namespace Ejercicio.Business
{
    using Ejercicio.Models.Api;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface ICRUDBusiness<TModel, TKey> : IDisposable where TModel : class, new()
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetAsync(TKey id);
        Task<bool> ExistAsync(TModel model);
        Task<bool> ExistAsync(TKey id);
        Task<TModel> SaveAsync(TModel model);
        Task<bool> DeleteAsync(TKey id);
    }
}
