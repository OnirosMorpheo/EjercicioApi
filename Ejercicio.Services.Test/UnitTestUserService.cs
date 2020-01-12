using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ejercicio.Entities;
using System.Globalization;
using System.Threading.Tasks;

namespace Ejercicio.Services.Test
{
    /// <summary>
    /// Descripción resumida de UnitTestUserService
    /// </summary>
    [TestClass]
    public class UnitTestUserService : UnitTestBase
    {
        private readonly IUserService userService;

        public UnitTestUserService() : base()
        {
            this.userService = ProbarResolucion<IUserService>();
        }

        [TestMethod]
        public void TestGetAll() {
            var llamada = Task.Run(() => this.userService.GetAllAsync());
            llamada.Wait();
            Assert.IsNotNull(llamada.Result);            
        }

        [TestMethod]
        public void TestGetByIdNull()
        {
            var llamada = Task.Run(() => this.userService.GetAsync(Guid.Empty));
            llamada.Wait();
            Assert.IsNull(llamada.Result);
        }

        [TestMethod]
        public void TestGetByIdNotNull()
        {
            var llamada = Task.Run(() => this.userService.GetAsync(Guid.Parse("946c2551-779a-4dfd-a539-23bf19719a16")));
            llamada.Wait();
            Assert.IsNotNull(llamada.Result);
        }

        [TestMethod]
        public void TestCrearNuevoUsurio()
        {
            UserDto userDto = new UserDto();
            var resultado = CrearNuevoUsuario(userDto);
            Assert.IsTrue(resultado);
            Assert.IsFalse(userDto.Uid == Guid.Empty);            
        }

        private bool CrearNuevoUsuario(UserDto userDto) {
            userDto.Uid = Guid.Empty;
            userDto.Name = "Pedro";
            userDto.Birthdate = DateTime.Parse("1986/06/30", CultureInfo.InvariantCulture);
            var llamada = Task.Run(() => this.userService.Save(userDto));
            llamada.Wait();
            var resultado = llamada.Result;
            return resultado;
        }

        [TestMethod]
        public void TestModificarUsurio()
        {
            var llamada = Task.Run(() => this.userService.GetAsync(Guid.Parse("946c2551-779a-4dfd-a539-23bf19719a16")));
            llamada.Wait();
            UserDto userDto = llamada.Result;
            Assert.IsNotNull(userDto);
            userDto.Name = "PruebaX";
            var llamada2 = Task.Run(() => this.userService.Save(userDto));
            llamada2.Wait();
            bool resultado = llamada2.Result;
            Assert.IsTrue(resultado);            
        }


        [TestMethod]
        public void TestEliminarUsurio()
        {
            UserDto userDto = new UserDto();
            bool resultado = CrearNuevoUsuario(userDto);
            Assert.IsTrue(resultado);
            Assert.IsNotNull(userDto);
            Assert.IsTrue(userDto.Uid != Guid.Empty);
            var llamada = Task.Run(() => this.userService.DeleteAsync(userDto.Uid));
            llamada.Wait();
            Assert.IsTrue(llamada.Result);
        }


    }
}
