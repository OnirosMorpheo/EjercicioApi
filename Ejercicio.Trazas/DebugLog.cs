
namespace Ejercicio.Trazas
{
    using System.Diagnostics;

    public class DebugLog : ITrazaLog, IExceptionLog
    {
        public bool SalvarLog(Traza traza)
        {
            try
            {
                Debug.WriteLine("--");
                Debug.WriteLine(traza.TextoSinSaltosDeLinea);
                Debug.WriteLine("--");

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
