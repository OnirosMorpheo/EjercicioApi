

namespace Ejercicio.WebApi.Test
{
    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;
    using Ejercicio.Infraestructure;
    using Ejercicio.Utilities;
    using Ejercicio.WebApi.Controllers;
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Mvc;

    public class AutofacConfig
    {
        public static IContainer Container { get; set; }

        public static void Initialize() {
            Container = RegisterService(new ContainerBuilder());
        }

        private static IContainer RegisterService(ContainerBuilder builder) {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().Cast<Assembly>().FirstOrDefault(elemento => elemento.FullName.StartsWith("Ejercicio.WebApi,"));

            //var aux = assembly.GetTypes().Where(elemento => elemento.Name.EndsWith("Controller"));

            builder.RegisterAssemblyTypes(assembly).Where(elemento => elemento.IsAssignableTo<ApiController>() || elemento.IsAssignableTo<Controller>()).AsSelf().InstancePerDependency();

            //Añadir las tipos aquí:

            builder.RegisterModule<UtilitiesModule>();
            builder.RegisterModule<ServicesLayerModule>();
            builder.RegisterModule<BusinessLayerModule>();
            builder.RegisterModule<WebApiLayerModule>();

            //Set the dependency resolver to be Autofac.  
            return builder.Build();

        }
    }
}
