

namespace Ejercicio.Utilities
{
    using Autofac;
    //using Ejercicio.Utilities.Helpers;
    public class UtilitiesModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterGeneric(typeof(HttpHelper<>)).As(typeof(IHttpHelper<>)).InstancePerDependency();
        }
    }
}

