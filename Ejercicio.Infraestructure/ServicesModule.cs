
namespace Ejercicio.Infraestructure
{
    using Autofac;
    using Autofac.Extras.DynamicProxy;
    using Ejercicio.Entities;
    using Ejercicio.Persistencia;
    using Ejercicio.Services;
    using Ejercicio.Trazas;

    public class ServicesLayerModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepoLog>().As<IRepoLog>().SingleInstance();

            builder.RegisterModule<PersistenciaModule>();
            builder.RegisterModule<TrazasModule>();

            //AsSelf() indica que no hay interfaz en la resolución si no que devuelve un objeto del mismo:  

            builder.RegisterType<ConfigurationUpruc>().AsSelf().SingleInstance();
            builder.RegisterType<ConfigurationPortaFirmas>().AsSelf().SingleInstance();
            builder.RegisterType<ConfigurationRldUsuario>().AsSelf().SingleInstance();


            builder.RegisterAssemblyTypes(typeof(Auditoria).Assembly)
                   .Where(t => t.Name.StartsWith("Fn"))
                   .AsSelf().InstancePerDependency()//.InstancePerRequest()
                   .EnableClassInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_2); ;

            builder.RegisterAssemblyTypes(typeof(ProcLiberarRegistroBorrado).Assembly)
                   .Where(elemento => elemento.Name.StartsWith("Proc"))
                   .AsSelf().InstancePerDependency()//.InstancePerRequest()
                   .EnableClassInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_2); ;

            builder.RegisterAssemblyTypes(typeof(TypeService).Assembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces().InstancePerDependency()//.InstancePerRequest()
                   .EnableInterfaceInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_2);

        }
    }
}
