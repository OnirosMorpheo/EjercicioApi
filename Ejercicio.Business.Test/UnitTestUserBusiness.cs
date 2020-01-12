using System;
using System.Globalization;
using System.Threading.Tasks;
using Ejercicio.Models.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ejercicio.Business.Test
{
    [TestClass]
    public class UnitTestUserBusiness : UnitTestBase
    {
        private readonly IUserBusiness userBusiness;

        public UnitTestUserBusiness() : base() {
            this.userBusiness = this.ProbarResolucion<IUserBusiness>();
        }


        [TestMethod]
        public void TestGetAll()
        {
            var llamada = Task.Run(() => this.userBusiness.GetAllAsync());
            llamada.Wait();
            Assert.IsNotNull(llamada.Result);
        }

        [TestMethod]
        public void TestGetByIdNull()
        {
            var llamada = Task.Run(() => this.userBusiness.GetAsync(Guid.Empty));
            llamada.Wait();
            Assert.IsNull(llamada.Result);
        }

        [TestMethod]
        public void TestGetByIdNotNull()
        {
            var llamada = Task.Run(() => this.userBusiness.GetAsync(Guid.Parse("946c2551-779a-4dfd-a539-23bf19719a16")));
            llamada.Wait();
            Assert.IsNotNull(llamada.Result);
        }

        [TestMethod]
        public void TestCrearNuevoUsurio()
        {
            UserModel userModel = CrearNuevoUsuario();
            Assert.IsNotNull(userModel);
            Assert.IsFalse(userModel.Uid == Guid.Empty);
        }

        private UserModel CrearNuevoUsuario()
        {
            UserModel userModel = new UserModel();
            userModel.Uid = Guid.Empty;
            userModel.Name = "Juan";
            userModel.Birthdate = DateTime.Parse("1986/06/30", CultureInfo.InvariantCulture);
            var llamada = Task.Run(() => this.userBusiness.SaveAsync(userModel));
            llamada.Wait();
            var resultado = llamada.Result;
            return resultado;
        }

        [TestMethod]
        public void TestModificarUsurio()
        {
            var llamada = Task.Run(() => this.userBusiness.GetAsync(Guid.Parse("946c2551-779a-4dfd-a539-23bf19719a16")));
            llamada.Wait();
            UserModel userModel = llamada.Result;
            Assert.IsNotNull(userModel);
            userModel.Name = "PruebaX2";
            var llamada2 = Task.Run(() => this.userBusiness.SaveAsync(userModel));
            llamada2.Wait();
            userModel = llamada2.Result;
            Assert.IsNotNull(userModel);
            Assert.IsTrue(userModel.Name == "PruebaX2");
        }


        [TestMethod]
        public void TestEliminarUsurio()
        {
            UserModel userModel = CrearNuevoUsuario();
            Assert.IsNotNull(userModel);            
            Assert.IsTrue(userModel.Uid != Guid.Empty);
            var llamada = Task.Run(() => this.userBusiness.DeleteAsync(userModel.Uid));
            llamada.Wait();
            Assert.IsTrue(llamada.Result);
        }



    }
}
