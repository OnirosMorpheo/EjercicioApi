
namespace Ejercicio.Business.Test
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTestAutofac : UnitTestBase
    {
        public UnitTestAutofac() : base()
        {

        }

        [TestMethod]
        public void ContainerTest()
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(elemento => elemento.GetName().Name == "Ejercicio.Business");
            Assert.IsNotNull(assembly, "No se ha encontrado el Assembly");

            foreach (Type tipo in assembly.GetTypes().Where(elemento => (elemento.Name.EndsWith("Business") || elemento.Name.EndsWith("Adapter")) && elemento.IsInterface))
            {
                var method = this.GetType().GetMethod("ProbarResolucion").MakeGenericMethod(tipo);
                var resultado = method.Invoke(this, new object[0]);
                Assert.IsNotNull(resultado);
            }
        }

    }
}
