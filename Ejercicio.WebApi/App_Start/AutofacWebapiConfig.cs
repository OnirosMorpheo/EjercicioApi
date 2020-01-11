
namespace Ejercicio.WebApi
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;
    using Ejercicio.Infraestructure;
    using System.Reflection;
    using System.Web.Http;
    using Ejercicio.Utilities;

    /// <summary>
    /// Configuración del contenedor de IoC
    /// </summary>
    public static class AutofacWebapiConfig
    {

        /// <summary>
        /// Contenedor de IoC
        /// </summary>
        public static IContainer Container;

        /// <summary>
        /// Inicializacion del contenedor
        /// </summary>
        /// <param name="config"></param>
        public static void Initialize(HttpConfiguration config)
        {
            Initialize(config, RegisterServices(new ContainerBuilder(), config));
        }

        /// <summary>
        /// Inicializa
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder, HttpConfiguration config)
        {
            //Register your Web API controllers.  
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            //Añadir las tipos aquí:

            builder.RegisterModule<UtilitiesModule>();
            builder.RegisterModule<ServicesLayerModule>();
            builder.RegisterModule<BusinessLayerModule>();
            builder.RegisterModule<WebApiLayerModule>();

            builder.RegisterWebApiFilterProvider(config);

            //Set the dependency resolver to be Autofac.  
            Container = builder.Build();

            return Container;
        }
    }
}