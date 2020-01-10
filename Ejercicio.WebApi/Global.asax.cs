
namespace Ejercicio.WebApi
{
    using log4net;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    /// <summary>
    /// WebApiApplication
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Event Application_Start
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MediaTypeFormatterCollection formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);


            formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            //formatters.JsonFormatter.SerializerSettings.Culture = CultureInfo.GetCultureInfo("es-ES");




            formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data"));

            MvcHandler.DisableMvcResponseHeader = true;


            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        /// <summary>
        /// Método tratamiento de codigos de error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            // Código que se ejecuta al producirse un error no controlado
            ILog logger = LogManager.GetLogger(typeof(WebApiApplication));
            if (logger != null)
            {
                Exception ex = Server.GetLastError();
                logger.ErrorFormat(CultureInfo.InvariantCulture, "Application_Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Event EndRequest
        /// </summary>
        protected void Application_EndRequest()
        {
            //removing excessive headers. They don't need to see this.
            Response.Headers.Remove("X-Powered-By");
            string value = Response.Headers.AllKeys.Where(elemento => elemento.Contains("4.0")).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(value)) Response.Headers.Remove(value);
        }
    }
}
