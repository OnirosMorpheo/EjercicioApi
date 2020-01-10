
namespace Ejercicio.Trazas
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Web.Hosting;
    public class FileLog : ITrazaLog, IExceptionLog
    {
        private string path = Path.Combine(HostingEnvironment.MapPath("~/App_Data/"), "Log.txt");
        private static ReaderWriterLock rwl = new ReaderWriterLock();
        public bool SalvarLog(Traza traza)
        {
            try
            {
                rwl.AcquireWriterLock(int.MaxValue);
                try
                {
                    File.AppendAllText(path, traza.TextoConSaltosDeLineaTabulados + Environment.NewLine);
                }
                finally
                {
                    rwl.ReleaseWriterLock();
                }
            }
            catch (ApplicationException)
            {
            }

            return true;
        }
    }
}
