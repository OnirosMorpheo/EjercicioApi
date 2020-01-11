

namespace Ejercicio.Services
{
    using Ejercicio.Entities;
    using Ejercicio.Persistence.Interfaces;
    using Ejercicio.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public class UserService : IUserService
    {
        private readonly IRepositorioGenerico<UserDto> repositorio;

        public UserService(IRepositorioGenerico<UserDto> repositorio) {
            this.repositorio = repositorio;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (Utils.Setting<bool>("BorradoLogico"))
                return await this.repositorio.SoftDeleteAsync(id);
            else
                return await this.repositorio.DeleteByUidAsync(id);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await this.repositorio.GetAllAsync(int.MaxValue);
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            return await this.repositorio.GetByUidAsync(id);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await this.repositorio.ExistsAsync(id);
        }

        public async Task<UserDto> GetAsync(UserDto entidad)
        {
            return await this.repositorio.GetByUidAsync(entidad.Uid);
        }

        public async Task<bool> Save(UserDto entidad)
        {
            if (entidad.Uid == Guid.Empty)
            {
                return await this.repositorio.InsertAsync(entidad);
            }
            else {
                return await this.repositorio.UpdateAsync(entidad);
            }
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

        ~UserService()
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
