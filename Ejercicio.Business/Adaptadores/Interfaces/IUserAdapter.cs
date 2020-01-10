

namespace Ejercicio.Business.Adaptadores
{
    using Ejercicio.Entities;
    using Ejercicio.Models.Api;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public interface IUserAdapter : IAdapter<UserDto, UserModel>
    {
    }
}
