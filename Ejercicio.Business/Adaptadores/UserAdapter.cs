

namespace Ejercicio.Business.Adaptadores
{
    using Autofac;
    using AutoMapper;
    using Ejercicio.Business.Adaptadores.Profiles;
    using Ejercicio.Entities;
    using Ejercicio.Models.Api;
    using System;
    class UserAdapter : Adapter<UserDto, UserModel>, IUserAdapter
    {
        protected override Action<IMapperConfigurationExpression> configExpresion
        {
            get
            {
                return cfg =>
                {
                    cfg.AddProfile<UserProfile>();
                };
            }
        }

        public UserAdapter(ILifetimeScope scope) : base(scope)
        {

        }
    }
}
