

namespace Ejercicio.Services.Test
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
            return builder.Build();
        }
    }
}
