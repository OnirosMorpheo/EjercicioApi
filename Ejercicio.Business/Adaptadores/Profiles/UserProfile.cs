
namespace Ejercicio.Business.Adaptadores.Profiles
{
    using AutoMapper;
    using Ejercicio.Entities;
    using Ejercicio.Models.Api;
    using Ejercicio.Utilities.Extensiones;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, UserModel>()
                .IgnoreAllNonExisting()
                .ForMember(opt => opt.Id,
                           src => src.Ignore());

            CreateMap<UserModel, UserDto>()
                .IgnoreAllNonExisting()
                .ForMember(opt => opt.Id,
                           src => src.Ignore());
        }
    }
}
