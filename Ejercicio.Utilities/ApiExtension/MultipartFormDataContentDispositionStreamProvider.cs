

namespace Ejercicio.Utilities.ApiExtension
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    public class MultipartFormDataContentDispositionStreamProvider : MultipartFormDataStreamProvider
    {
        public MultipartFormDataContentDispositionStreamProvider(string rootPath) : base(rootPath)
        {

        }
        public MultipartFormDataContentDispositionStreamProvider(string rootPath, int bufferSize) : base(rootPath, bufferSize)
        {

        }
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            if (headers.ContentDisposition != null)
            {
                return headers.ContentDisposition.FileName;
            }
            return base.GetLocalFileName(headers);
        }
    }
}
