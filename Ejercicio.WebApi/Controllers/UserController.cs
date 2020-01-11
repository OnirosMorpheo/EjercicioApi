
namespace Ejercicio.WebApi.Controllers
{
    using Ejercicio.Business;
    using Ejercicio.Models.Api;
    using Microsoft.Web.Http;
    using System;
    using System.Web.Http;

    /// <summary>
    /// Controlador de Usuario
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/Users")]
    public class UserController : CRUDController<UserModel, Guid>
    {

        /// <summary>
        /// Constructor parametrizado del controlador
        /// </summary>
        /// <param name="business"></param>
        public UserController(ICRUDBusiness<UserModel, Guid> business) : base(business) { 
        
        }
    }
}
