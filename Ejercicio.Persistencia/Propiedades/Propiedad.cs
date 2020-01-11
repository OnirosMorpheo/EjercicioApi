

namespace Ejercicio.Persistence.Propiedades
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    public class Propiedad
    {
        private const string regxFK = @"\[FK:([^\]]*)\]";
        private const string regxPK = @"\[PK:([^\]]*)\]";
        private const string regxOI = @"\[OnlyInsert\]";

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionRelacionFK { get; set; }
        public string DescripcionRelacionPK { get; set; }
        public string Tipo { get; set; }
        public List<string> Valores { get; set; }
        public bool Ordenable { get; set; }
        public bool EsNulable { get; set; }
        public int? MaxLength { get; set; }

        public Propiedad(string nombre, string tipo, string descripcion, bool esNulable, int? maxLength)
        {
            Nombre = nombre;
            Tipo = tipo;
            Descripcion = string.Empty;
            DescripcionRelacionFK = string.Empty;
            DescripcionRelacionPK = string.Empty;
            var descripcionDB = descripcion ?? string.Empty;
            if (!string.IsNullOrEmpty(descripcionDB))
            {
                Regex rFK = new Regex(regxFK);
                Match mFK = rFK.Match(descripcionDB);
                if (mFK.Success)
                {
                    DescripcionRelacionFK = mFK.Success ? mFK.Groups[1].Value : string.Empty;
                    descripcionDB = descripcionDB.Replace(mFK.Value, string.Empty);
                }

                Regex rPK = new Regex(regxPK);
                Match mPK = rPK.Match(descripcionDB);
                if (mPK.Success)
                {
                    DescripcionRelacionPK = mPK.Success ? mPK.Groups[1].Value : string.Empty;
                    descripcionDB = descripcionDB.Replace(mPK.Value, string.Empty);
                }

                Regex rOI = new Regex(regxOI);
                Match mOI = rOI.Match(descripcionDB);
                if (mOI.Success)
                {
                    descripcionDB = descripcionDB.Replace(mOI.Value, string.Empty);
                }

                Descripcion = descripcionDB;
            }
        }
    }
}
