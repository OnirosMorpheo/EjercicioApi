

namespace Ejercicio.Utilities.Extensions
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    public static class HttpClientExtensions
    {
        //
        // Parámetros de tipo:
        //   T:
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient client, string url, T value) {
            string data = JsonConvert.SerializeObject(value);
            Uri requestUri = new Uri(client.BaseAddress, url);
            HttpResponseMessage result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri)
            {
                Content = new StringContent(data, Encoding.UTF8, "application/json")
            }); 
            return result;
        }

        //public static async Task<HttpResponseMessage> PostAsJsonFormDataAsync<T>(this HttpClient, string url, T value)

    }
}
