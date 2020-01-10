
namespace Ejercicio.WebApi.Controllers
{
    using Ejercicio.Utilities.ApiExtension;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Routing;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bodyContent"></param>
        /// <param name="cabeceras"></param>
        /// <returns></returns>
        protected IHttpActionResult RldLogin(dynamic bodyContent, Dictionary<string, string> cabeceras)
        {
            return new HttpResult(base.Request, HttpStatusCode.OK, bodyContent, cabeceras);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="viewName"></param>
        /// <param name="viewData"></param>
        /// <returns></returns>
        protected static string RenderViewToString(string controllerName, string viewName, System.Web.Mvc.ViewDataDictionary viewData)
        {
            using (var writer = new StringWriter())
            {
                var routeData = new RouteData();
                routeData.Values.Add("controller", controllerName);
                var fakeControllerContext = new System.Web.Mvc.ControllerContext(new HttpContextWrapper(new HttpContext(new HttpRequest(null, "http://google.com", null), new HttpResponse(null))), routeData, new FakeController());
                var razorViewEngine = new System.Web.Mvc.RazorViewEngine();
                var razorViewResult = razorViewEngine.FindView(fakeControllerContext, viewName, "", false);

                var viewContext = new System.Web.Mvc.ViewContext(fakeControllerContext, razorViewResult.View, viewData, new System.Web.Mvc.TempDataDictionary(), writer);
                razorViewResult.View.Render(viewContext, writer);
                return writer.ToString();
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class FakeController : System.Web.Mvc.ControllerBase { protected override void ExecuteCore() { } }
}