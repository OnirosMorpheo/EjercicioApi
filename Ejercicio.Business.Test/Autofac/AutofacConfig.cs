

namespace Ejercicio.Business.Test
{
    using Autofac;
    using Ejercicio.Infraestructure;
    using Ejercicio.Utilities;
    public class AutofacConfig
    {
        public static IContainer Container { get; set; }

        public static void Initialize() {
            Container = RegisterService(new ContainerBuilder());
        }

        private static IContainer RegisterService(ContainerBuilder builder) {
            builder.RegisterModule<UtilitiesModule>();
            builder.RegisterModule<ServicesLayerModule>();
            builder.RegisterModule<BusinessLayerModule>();
            return builder.Build();
        }
    }
}
