

namespace Ejercicio.Persistence
{
    using System;
    public class EntidadClaveExterna
    {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public Guid? Uid { get; set; }
        /// <summary>
        /// Nombre de la entidad
        /// </summary>
        public string Nombre { get; set; }
        public EntidadClaveExterna()
        {

        }
        public EntidadClaveExterna(Guid? Uid, string nombre = "")
        {
            this.Uid = Uid;
            Nombre = nombre;
        }
    }
}
