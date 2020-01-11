

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
        private readonly IUserService userService;
        private readonly IUserAdapter adapter;
        
        public UserBusiness(IUserService userService, IUserAdapter adapter)
        {
            this.userService = userService;
            this.adapter = adapter;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return adapter.ToIEnumerableModel(await userService.GetAllAsync());
        }

        public async Task<UserModel> GetAsync(Guid id)
        {
            return adapter.ToModel(await userService.GetAsync(id));
        }

        public async Task<bool> ExistAsync(UserModel model)
        {
            return await userService.ExistsAsync(model.Id);
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            return await userService.ExistsAsync(id);
        }

        public async Task<UserModel> SaveAsync(UserModel model)
        {
            UserDto entidad = adapter.ToDto(model);
            if (await this.userService.Save(entidad))
                model.Uid = entidad.Uid;
            else
                return null;
            return model;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await userService.DeleteAsync(id);
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
