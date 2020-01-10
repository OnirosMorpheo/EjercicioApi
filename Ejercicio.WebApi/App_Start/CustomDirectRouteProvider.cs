
namespace Ejercicio.WebApi
{
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;
    /// <summary>
    /// 
    /// </summary>
    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            // inherit route attributes decorated on base class controller's actions
            return actionDescriptor is null ? null : actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
        }
    }
}