

namespace Ejercicio.Services.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    [TestClass]
    public class UnitTestAutofac : UnitTestBase
    {
        public UnitTestAutofac() : base()
        {

        }

        [TestMethod]
        public void ContainerTest()
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(elemento => elemento.GetName().Name == "Ejercicio.Services");
            Assert.IsNotNull(assembly, "No se ha encontrado el Assembly");

            foreach (Type tipo in assembly.GetTypes().Where(elemento => elemento.Name.EndsWith("Service") && elemento.IsInterface))
            {
                var method = this.GetType().GetMethod("ProbarResolucion").MakeGenericMethod(tipo);
                var resultado = method.Invoke(this, new object[0]);
                Assert.IsNotNull(resultado);
            }
        }
    
    }
}
