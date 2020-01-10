

namespace Ejercicio.Business
{
    using Ejercicio.Business.Adaptadores;
    using Ejercicio.Entities;
    using Ejercicio.Models.Api;
    using Ejercicio.Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserBusiness : IUserBusiness, IDisposable
    {
        private readonly IUserServices service;
        private readonly IUserAdapter adapter;
        
        public UserBusiness(IUserServices service, IUserAdapter adapter)
        {
            this.service = service;
            this.adapter = adapter;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync(FiltroModel filtro = null)
        {
            return adapter.ToIEnumerableModel(await Task.Run(() => service.GetAll()));
        }

        public async Task<UserModel> GetAsync(int id)
        {
            return adapter.ToModel(await Task.Run(() => service.Get(id)));
        }

        public Task<bool> ExistAsync(UserModel model)
        {
            return Task.Run(() => service.Get(adapter.ToDto(model)) != null);
        }

        public Task<bool> ExistAsync(int id)
        {
            return Task.Run(() => service.Get(id) != null);
        }

        public async Task<UserModel> SaveAsync(UserModel model)
        {
            UserDto entidad = adapter.ToDto(model);
            if (await Task.Run(() => this.service.Save(entidad)))
                model.Uid = entidad.Uid;
            else
                return null;
            return model;
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.Run(() => this.service.Delete(id));
        }

        #region Patron Disposable

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose-only, i.e. non-finalizable logic

                }

                // shared cleanup logic
                disposed = true;
            }
        }

        ~UserBusiness()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    
    }
}
