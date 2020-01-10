

namespace Ejercicio.WebApi
{
    using Ejercicio.Trazas;
    using Ejercicio.Utilities;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;
    using System.Web.Mvc;

    /// <summary>
    /// Attribute for ApiException
    /// </summary>
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {

        /// <summary>
        /// Event OnException ApiExceptionAttribute
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            var logger = DependencyResolver.Current.GetService<TrazaLoggerInterceptor>();

            var idException = Guid.NewGuid();
            var exception = context is null ? string.Empty : context.Exception.GetType().FullName;

            logger.GuardarExcepcion(idException, UsuarioContexto.Nombre, "WebApi", exception, "", "ApiExceptionAttribute", context.Exception);

            if (context.Exception is ValidationDBException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = "Error en la validación de la entidad.",
                    Content = new StringContent(context.Exception.Message + "\r\n\r\nCorrija los errores o contacte con el administrador.")
                };
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "No autorizado.",
                    Content = new StringContent("Creadenciales incorrectas / Petición no autorizada / Contenido no autorizado / Acción no disponible temporalmente.")
                };
            }
            else if (context.Exception is SecurityTokenValidationException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    ReasonPhrase = "Token no valido.",
                    Content = new StringContent("Error en la validación del token. ")
                };
            }
            else if (context.Exception is HttpRequestException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    ReasonPhrase = "Documentos no enlazados.",
                    Content = new StringContent(context.Exception.Message)
                };
            }
            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                            @"
                            Error interno. 
                            Vuelva a intentarlo o contacte con el administrador.
                            Identificador del error: " + idException)
                };
            }

            context.Response.Headers.Add("X-IdError", idException.ToString());
        }
    }
}