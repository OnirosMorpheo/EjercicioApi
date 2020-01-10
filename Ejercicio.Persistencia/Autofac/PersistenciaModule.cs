

namespace Ejercicio.Persistencia
{
    using Autofac;
    using Autofac.Extras.DynamicProxy;
    using Ejercicio.Persistencia.Interfaces;
    using Ejercicio.Trazas;

    public class PersistenciaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>().As<IDatabaseConnectionFactory>().SingleInstance();
            builder.RegisterType<RepositorioGenericoFactory>().As<IRepositorioGenericoFactory>().SingleInstance();
            builder.RegisterSource(new RepositorioGenericoRegistrationSource());
            builder.RegisterType<Repositorio>().As<IRepositorio>()/*.InstancePerRequest()//.InstancePerRequest()*/
                    .EnableInterfaceInterceptors().InterceptedBy(TrazaLoggerInterceptor.TRAZA_3);           
         
            
        }
    }
}
