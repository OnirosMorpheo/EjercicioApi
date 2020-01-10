
namespace Ejercicio.Utilities.ApiExtension
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    /// <summary>
    /// 
    /// </summary>
    public class HttpResult : IHttpActionResult
    {
        private readonly HttpRequestMessage request;
        private readonly HttpStatusCode code;
        private readonly Dictionary<string, string> cabeceras;
        private readonly dynamic content;
        /// <summary>
        /// Constructor de clase
        /// </summary>
        /// <param name="request"></param>
        /// <param name="code"></param>
        /// <param name="bodyContent"></param>
        /// <param name="cabeceras"></param>
        public HttpResult(HttpRequestMessage request, HttpStatusCode code, dynamic bodyContent, Dictionary<string, string> cabeceras)
        {
            this.code = code;
            this.cabeceras = cabeceras;
            this.content = bodyContent;
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
            httpResponse.Content = new StringContent(JsonConvert.SerializeObject(this.content), System.Text.Encoding.UTF8, "application/json");
            foreach (var cabecera in cabeceras)
            {
                httpResponse.Headers.Add(cabecera.Key, cabecera.Value);
            }
            return Task.FromResult(httpResponse);
        }

    }
}