
namespace Ejercicio.Business
{
    using Ejercicio.Models.Api;
    using System;

    public interface IUserBusiness : ICRUDBusiness<UserModel, Guid>, IDisposable
    {

    }
}
