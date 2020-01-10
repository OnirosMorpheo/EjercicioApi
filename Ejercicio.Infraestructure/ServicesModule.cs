
namespace Ejercicio.Infraestructure
{
    using Autofac;
    using Autofac.Extras.DynamicProxy;
    using Ejercicio.Entities;
    using Ejercicio.Persistencia;
    using Ejercicio.Trazas;
    using Ejercicio.Services;

    public class ServicesLayerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepoLog>().As<IRepoLog>().SingleInstance();

            builder.RegisterModule<PersistenciaModule>();
            builder.RegisterModule<TrazasModule>();
                       
            builder.RegisterAssemblyTypes(typeof(Auditoria).Assembly)
                   .Where(t => t.Name.StartsWith("Fn") || t.Name.StartsWith("Proc"))
                   .AsSelf().InstancePerDependency()//.InstancePerRequest()
                   .EnableClassInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_2); ;

            builder.RegisterAssemblyTypes(typeof(IUserServices).Assembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces().InstancePerDependency()//.InstancePerRequest()
                   .EnableInterfaceInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_2);

        }
    }
}
