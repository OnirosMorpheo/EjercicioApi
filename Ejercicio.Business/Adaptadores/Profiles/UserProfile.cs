
namespace Ejercicio.Business.Adaptadores.Profiles
{
    using AutoMapper;
    using Ejercicio.Entities;
    using Ejercicio.Models.Api;
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserModel>();

            CreateMap<UserModel, UserDto>();
        }
    }
}
