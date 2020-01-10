

namespace Ejercicio.Persistencia
{
    using Autofac;
    using Autofac.Core;
    using Autofac.Core.Activators.Delegate;
    using Autofac.Core.Lifetime;
    using Autofac.Core.Registration;
    using Ejercicio.Persistencia.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public class RepositorioGenericoRegistrationSource : IRegistrationSource
    {
        public bool IsAdapterForIndividualComponents { get { return false; } }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
#pragma warning disable IDE0019 // Usar coincidencia de patrones
            IServiceWithType swt = service as IServiceWithType;
#pragma warning restore IDE0019 // Usar coincidencia de patrones

            if (swt == null || swt.ServiceType.Name != "IRepositorioGenerico`1")
            {
                // It's not a request for the base handler type, so skip it.
                return Enumerable.Empty<IComponentRegistration>();
            }

            IComponentRegistration registration = new ComponentRegistration(
              Guid.NewGuid(),
              new DelegateActivator(swt.ServiceType, (c, p) =>
              {
                  // In this example, the factory itself is assumed to be registered
                  // with Autofac, so we can resolve the factory. If you want to hard
                  // code the factory here, you can do that, too.
                  var factoria = c.Resolve<IRepositorioGenericoFactory>();

                  // Our factory interface is generic, so we have to use a bit of
                  // reflection to make the call.
                  var metodo = factoria.GetType().GetMethod("GetRepositorio").MakeGenericMethod(swt.ServiceType.GetGenericArguments().First());

                  // In the end, return the object from the factory.
                  return metodo.Invoke(factoria, null);
              }),
              new CurrentScopeLifetime(),
              InstanceSharing.None,
              InstanceOwnership.OwnedByLifetimeScope,
              //InstanceOwnership.ExternallyOwned,
              new[] { service },
              new Dictionary<string, object>());

            return new IComponentRegistration[] { registration };
        }
    }
}
