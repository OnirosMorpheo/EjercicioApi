
namespace Ejercicio.Utilities.ApiExtension
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// Devuelve un resultado con código personalizado
    /// </summary>
    public class HttpStatusCodeResult : IHttpActionResult
    {
        private readonly HttpRequestMessage request;
        private readonly HttpStatusCode code;
        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="request"></param>
        /// <param name="code"></param>
        public HttpStatusCodeResult(HttpRequestMessage request, HttpStatusCode code)
        {
            this.code = code;
            this.request = request;
        }

        /// <summary>
        /// Implementación del método ExecuteAsync
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponse = request.CreateResponse(code);
            return Task.FromResult(httpResponse);
        }

    }
}