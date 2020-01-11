
namespace Ejercicio.Entities
{
    using Ejercicio.Persistence.Interfaces;
    using Ejercicio.Trazas;

    public class RepoLog : IRepoLog
    {
        private IRepositorioGenerico<TrazaDto> repositorioTraza { get; set; }
        public RepoLog(IRepositorioGenerico<TrazaDto> repositorioTraza)
        {
            this.repositorioTraza = repositorioTraza;
        }

        public bool Insert(Traza traza)
        {
            return repositorioTraza.Insert(traza);
        }
    }
}
