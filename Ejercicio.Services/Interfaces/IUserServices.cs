

namespace Ejercicio.Services
{
    using Ejercicio.Entities;
    using System;
    using System.Collections.Generic;

    public interface IUserServices : IDisposable
    {
        bool Delete(int id);
        IEnumerable<UserDto> GetAll();
        UserDto Get(int id);
        UserDto Get(UserDto entidad);
        bool Save(UserDto entidad);
    }
}
