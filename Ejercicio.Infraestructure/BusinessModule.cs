

namespace Ejercicio.Infraestructure
{
    using Autofac;
    using Autofac.Extras.DynamicProxy;
    using Ejercicio.Business;
    using Ejercicio.Business.Adaptadores;
    using Ejercicio.Business.Adaptadores.Profiles;
    using Ejercicio.Trazas;

    public class BusinessLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Register types business layer:
            builder.RegisterAssemblyTypes(typeof(UserProfile).Assembly)
                   .Where(t => t.Name.EndsWith("Profile"))
                   .AsSelf().SingleInstance()
                   .EnableClassInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_3);

            builder.RegisterAssemblyTypes(typeof(IUserAdapter).Assembly)
                   .Where(t => t.Name.EndsWith("Adapter"))
                   .AsImplementedInterfaces().SingleInstance()
                    .EnableInterfaceInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_3);

            builder.RegisterAssemblyTypes(typeof(IUserBusiness).Assembly)
                   .Where(t => t.Name.EndsWith("Business"))
                   .AsImplementedInterfaces().InstancePerDependency()
                   .EnableInterfaceInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_3);
        }
    }
}
