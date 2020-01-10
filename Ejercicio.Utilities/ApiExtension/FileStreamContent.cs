

namespace Ejercicio.Utilities.ApiExtension
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;

    public class FileStreamContentResult : IHttpActionResult
    {
        private string _filePath;
        private string _contentType;
        private string _filename;
        private Stream _stream;

        private readonly HttpClient _client;
        private readonly HttpResponseMessage _message;

        public FileStreamContentResult(string filePath, string contentType = null, string filename = null)
        {
            if (filePath == null) throw new ArgumentNullException("filePath");

            _filePath = filePath;
            _contentType = contentType;
            _filename = filename;
        }

        public FileStreamContentResult(Stream stream, string contentType = null, string filename = null)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            _stream = stream;
            _contentType = contentType;
            _filename = filename.Substring(1, filename.Length - 2);
        }

        public FileStreamContentResult(HttpClient client, HttpResponseMessage message)
        {
            this._client = client;
            this._message = message;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            if (this._client != null && this._message != null) {
                if (!this._message.IsSuccessStatusCode || this._message.StatusCode == HttpStatusCode.NoContent) return await Task.FromResult(new HttpResponseMessage(this._message.StatusCode));
                _stream = await _message.Content.ReadAsStreamAsync();
                
                _contentType = _message.Content.Headers?.ContentType?.MediaType;
                _filename = _message.Content.Headers?.ContentDisposition?.FileName;
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(_stream ?? File.OpenRead(_filePath)),
                
            };

            var contentType = _contentType ?? MimeMapping.GetMimeMapping(Path.GetExtension(_filePath));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            if (!string.IsNullOrWhiteSpace(_filename))
            {
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = _filename
                };
            }

            return await Task.FromResult(response);
        }
    }
}
