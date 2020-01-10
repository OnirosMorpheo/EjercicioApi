
namespace Ejercicio.WebApi
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Filtro de validación para el retorno automático de conflict cuando el modelo no es válido.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Sobrecarga del método OnActionExecuting
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Conflict, actionContext.ModelState);
            }
        }
    }
}
}