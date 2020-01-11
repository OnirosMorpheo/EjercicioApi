
namespace Ejercicio.Trazas
{
    using System.Diagnostics;

    public class WindowsEventLog : ITrazaLog, IExceptionLog
    {
        private static string source = "WebApi";
        private static string log = "Ejercicio";
        public bool SalvarLog(Traza traza)
        {
            try
            {
                var appLog = new EventLog(log);
                appLog.Source = source;
                appLog.WriteEntry(traza.TextoConSaltosDeLinea, traza.EsExcepcion ? EventLogEntryType.Error : EventLogEntryType.Information);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
