
namespace Ejercicio.WebApi.Controllers
{
    using Ejercicio.Utilities.ApiExtension;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    /// <summary>
    /// Controlador base que hereda de ApiController
    /// </summary>

    [ApiExceptionAttribute]
    public class BaseController : ApiController
    {
        /// <summary>
        /// Método Options devolviendo status OK
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public HttpResponseMessage Options()
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
            return response;
        }

        /// <summary>
        /// Método devolución HttpStatusCodeResult for NoContent
        /// </summary>
        /// <returns></returns>
        protected IHttpActionResult NoContent()
        {
            return new HttpStatusCodeResult(base.Request, HttpStatusCode.NoContent);
        }

    }
        
}