
namespace Ejercicio.Utilities.ApiExtension
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class CustomAutorizeAttribute : AuthorizeAttribute
    {   
        protected override Boolean IsAuthorized(HttpActionContext actionContext)
        {
            if (Utils.Setting<bool>("JWT_ACTIVADO"))
                return base.IsAuthorized(actionContext);
            else
                return true;
        }
    }
}
