

namespace Ejercicio.WebApi.Test
{
    using Autofac;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    [TestClass]
    public class UnitTestBase
    {
        protected readonly IContainer container;

        public UnitTestBase() {
            AutofacConfig.Initialize();
            this.container = AutofacConfig.Container;
        }

        public T ProbarResolucion<T>() where T : class {
            T objTest = null;
            try
            {
                objTest = container.Resolve<T>();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsNotNull(objTest);
            Type typeGenerate = objTest.GetType();
            Assert.IsTrue(typeof(T) == objTest.GetType() || typeGenerate.GetInterfaces().Contains(typeof(T)));
            return objTest;
        }

    }
}
