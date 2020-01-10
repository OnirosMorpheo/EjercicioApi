
namespace Ejercicio.Trazas
{
    using System;

    public class DbLog : ITrazaLog, IExceptionLog
    {
        public IRepoLog RepositorioTraza { get; set; }

        public DbLog()
        {
            
        }

        public bool SalvarLog(Traza traza)
        {
            try
            {
                return RepositorioTraza.Insert(traza);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
