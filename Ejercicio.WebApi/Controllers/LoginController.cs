

namespace Ejercicio.WebApi.Controllers
{
    using Ejercicio.Models.Api;
    using Ejercicio.Utilities;
    using Microsoft.Web.Http;
    using System.Web.Http;
    /// <summary>
    /// Controlador de logueo   
    /// </summary>
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/login")]
    public class LoginController : ApiController
    {

        /// <summary>
        /// Método obtencion de token JWT
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [Route("obtenerToken")]
        public IHttpActionResult ObtenerToken([FromBody]LoginModel login) {
            if (!Utils.Setting<bool>("JWT_ACTIVADO")) return NotFound();
            if (login.User == "admin" && login.Password == "123456") {
                string token = TokenGenerator.GenerateTokenJwt(login);
                return Ok(token);
            }
            return Unauthorized();
        }

    }
}
