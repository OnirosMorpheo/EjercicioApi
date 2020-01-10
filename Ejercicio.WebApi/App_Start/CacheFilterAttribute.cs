
namespace Ejercicio.WebApi
{
    using System;
    using System.Net.Http.Headers;
    using System.Web.Http.Filters;

    /// <summary>
    /// Filtro para agregar el cacheado de metodos
    /// </summary>
    public class CacheFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Duración de la cache
        /// </summary>
        public int TimeDuration { get; set; }
        /// <summary>
        /// Método sobreescrito OnActionExecuted
        /// </summary>
        /// <param name="actionExecutedContext">Contexto</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromSeconds(TimeDuration),
                MustRevalidate = true,
                Public = true
            };
        }
    }
}