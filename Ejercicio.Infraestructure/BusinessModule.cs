

namespace Ejercicio.Infraestructure
{
    using Autofac;
    using Autofac.Extras.DynamicProxy;
    usingEjercicio.Business;
    usingEjercicio.Business.Adaptadores;
    usingEjercicio.Trazas;
using Ejercicio.Trazas;

    public class BusinessLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //Register types business layer:
            builder.RegisterAssemblyTypes(typeof(RegPapeleraProfile).Assembly)
                   .Where(t => t.Name.EndsWith("Profile"))
                   .AsSelf().SingleInstance()
                   .EnableClassInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_3);

            builder.RegisterAssemblyTypes(typeof(IQueryAdapter).Assembly)
                   .Where(t => t.Name.EndsWith("Adapter"))
                   .AsImplementedInterfaces().SingleInstance()
                    .EnableInterfaceInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_3);

            builder.RegisterAssemblyTypes(typeof(IUprucBusiness).Assembly)
                   .Where(t => t.Name.EndsWith("Business"))
                   .AsImplementedInterfaces().InstancePerRequest()
                   .EnableInterfaceInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_3);
        }
    }
}
