
namespace Ejercicio.Trazas
{
    using Autofac;
    using Castle.DynamicProxy;
    using Ejercicio.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TrazasModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder) {

            /* Traza */
            builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_0, c.Resolve<IEnumerable<ITrazaLog>>(), c.Resolve<IEnumerable<IExceptionLog>>())).Named<IInterceptor>(TrazaLoggerInterceptor.TRAZA_0);//.SingleInstance();
            builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_1, c.Resolve<IEnumerable<ITrazaLog>>(), c.Resolve<IEnumerable<IExceptionLog>>())).Named<IInterceptor>(TrazaLoggerInterceptor.TRAZA_1);//.SingleInstance();
            builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_2, c.Resolve<IEnumerable<ITrazaLog>>(), c.Resolve<IEnumerable<IExceptionLog>>())).Named<IInterceptor>(TrazaLoggerInterceptor.TRAZA_2);//.SingleInstance();
            builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_3, c.Resolve<IEnumerable<ITrazaLog>>(), c.Resolve<IEnumerable<IExceptionLog>>())).Named<IInterceptor>(TrazaLoggerInterceptor.TRAZA_3);//.SingleInstance();
            builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_4, c.Resolve<IEnumerable<ITrazaLog>>(), c.Resolve<IEnumerable<IExceptionLog>>())).Named<IInterceptor>(TrazaLoggerInterceptor.TRAZA_4);//.SingleInstance();
            builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_5, c.Resolve<IEnumerable<ITrazaLog>>(), c.Resolve<IEnumerable<IExceptionLog>>())).Named<IInterceptor>(TrazaLoggerInterceptor.TRAZA_5);//.SingleInstance();
            builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_0, c.Resolve<IEnumerable<ITrazaLog>>(), c.Resolve<IEnumerable<IExceptionLog>>()));//.SingleInstance();
            //builder.Register(c => new TrazaLoggerInterceptor(TrazaLoggerInterceptor.TRAZA_NIVEL_0, new List<ITrazaLog>(), new List<IExceptionLog>()));//.SingleInstance();



            /* Traza */

            var sistemasRegistroTrazas = Utils.SettingList<string>("SistemasRegistroTrazas").Select(elemento => elemento.ToLower());
            var sistemasRegistroExcepciones = Utils.SettingList<string>("SistemasRegistroExcepciones").Select(elemento => elemento.ToLower());

            var typeException = typeof(IExceptionLog);
            var typeTraza = typeof(ITrazaLog);

            var ensamblados = /*BuildManager.GetReferencedAssemblies()*/AppDomain.CurrentDomain.GetAssemblies().Cast<Assembly>()
                .ToArray().Where(e => e.FullName.StartsWith("Ejercicio"));
            var tipos = ensamblados.SelectMany(s => s.GetTypes());

            var tiposException = tipos.Where(p => sistemasRegistroExcepciones.Contains(p.Name.ToLower()) && typeException.IsAssignableFrom(p) && typeException != p);
            var tiposTraza = tipos.Where(p => sistemasRegistroTrazas.Contains(p.Name.ToLower()) && typeTraza.IsAssignableFrom(p) && typeTraza != p);

            tiposException.ToList().ForEach(te =>
            {
                builder.RegisterType(te).As<IExceptionLog>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).SingleInstance();
            });

            tiposTraza.ToList().ForEach(tt =>
            {
                builder.RegisterType(tt).As<ITrazaLog>().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies).SingleInstance();
            });

        }
    }
}
