using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using Ejercicio.Models.Api;
using Ejercicio.WebApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ejercicio.WebApi.Test
{
    [TestClass]
    public class UnitTestUserController : UnitTestBase
    {
        

        public UnitTestUserController() : base() {
        
        }

        [TestMethod]
        public void UnitTestGetAllAsync() {
            UserController userController = ProbarResolucion<UserController>();
            userController.Request = new System.Net.Http.HttpRequestMessage();
            userController.Configuration = new System.Web.Http.HttpConfiguration();

            var llamada = Task.Run(() => userController.Get());
            llamada.Wait();
            IHttpActionResult response = llamada.Result;
            Assert.IsTrue(response.GetType() == typeof(OkNegotiatedContentResult<IEnumerable<UserModel>>));
            IEnumerable<UserModel> users = ((OkNegotiatedContentResult<IEnumerable<UserModel>>)response).Content;
            Assert.IsTrue(users.Any());
            
        }

        [TestMethod]
        public void UnitTestGetAsync()
        {
            UserController userController = ProbarResolucion<UserController>();
            userController.Request = new System.Net.Http.HttpRequestMessage();
            userController.Configuration = new System.Web.Http.HttpConfiguration();

            var llamada = Task.Run(() => userController.Get(Guid.Parse("946c2551-779a-4dfd-a539-23bf19719a16")));
            llamada.Wait();
            IHttpActionResult response = llamada.Result;
            Assert.IsTrue(response.GetType() == typeof(OkNegotiatedContentResult<UserModel>));
            UserModel user = ((OkNegotiatedContentResult<UserModel>)response).Content;
            Assert.IsNotNull(user);

        }

        [TestMethod]
        public void UnitTestPost() {
            UserController userController = ProbarResolucion<UserController>();

            userController.Request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://localhost/api/v1.0/users")
            };
            // Act
            UserModel user = new UserModel() { Name = "pepe", Birthdate = DateTime.Parse("1986/06/30") };
            var llamada = Task.Run(() => userController.Post(user));
            llamada.Wait();
            var response = llamada.Result;
            Assert.IsTrue(response.GetType() == typeof(CreatedNegotiatedContentResult<UserModel>));
        }

        [TestMethod]
        public void UnitTestPut()
        {
            UserController userController = ProbarResolucion<UserController>();

            userController.Request = new HttpRequestMessage() {
                RequestUri = new Uri("http://localhost/api/v1.0/users")
            };
            // Act
            UserModel user = new UserModel() { Id = Guid.Parse("946c2551-779a-4dfd-a539-23bf19719a16"), Name = "Prueba1", Birthdate = DateTime.Parse("1986/06/30") };
            var llamada = Task.Run(() => userController.Put(user.Id, user));
            llamada.Wait();
            var response = llamada.Result;
            Assert.IsTrue(response.GetType() == typeof(CreatedNegotiatedContentResult<UserModel>));
        }

        [TestMethod]
        public void UnitTestDelete()
        {
            // creo uno:
            UserController userController = ProbarResolucion<UserController>();

            userController.Request = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://localhost/api/v1.0/users")
            };
            // Act
            UserModel user = new UserModel() { Name = "pepe", Birthdate = DateTime.Parse("1986/06/30") };
            var llamada = Task.Run(() => userController.Post(user));
            llamada.Wait();
            var response = llamada.Result;
            Assert.IsTrue(response.GetType() == typeof(CreatedNegotiatedContentResult<UserModel>));
            

            user = ((CreatedNegotiatedContentResult<UserModel>)response).Content;
            Assert.IsTrue(user.Id != Guid.Empty);
            
            var llamada1 = Task.Run(() => userController.Delete(user.Id));
            llamada1.Wait();
            var responseDelete = llamada1.Result;
            Assert.IsTrue(responseDelete.GetType() == typeof(OkResult));

        }


    }
}
