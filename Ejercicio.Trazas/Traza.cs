

namespace Ejercicio.Trazas
{
    using System;
    public class Traza
    {
        private static string FORMATO = "{1} {2} -> {0}Uid: {3} {0}UidPeticion: {4} {0}Nick: {5} {0}Nivel: {6} {0}Dll: {7} {0}Descripcion: {8} {0}Fullname: {9} {0}Name: {10} {0}NameSpace: {11} {0}Parametros: {12} \n";

        public Guid Uid { get; set; }
        public Guid UidPeticion { get; set; }
        public DateTime CreadoEn { get; set; }
        public string Dll { get; set; }
        public bool EsExcepcion { get; set; }
        public string Descripcion { get; set; }
        public string Fullname { get; set; }
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string Nick { get; set; }
        public int Nivel { get; set; }
        public string Parametros { get; set; }
        public Traza()
        {
            CreadoEn = DateTime.UtcNow;
            Nivel = -1;
        }

        private string FormatearExcepcion(string separador)
        {
            return string.Format(FORMATO,
                                separador,
                                CreadoEn,
                                EsExcepcion ? "[EXCEPCION]" : "[TRAZA]",
                                Uid,
                                UidPeticion,
                                Nick,
                                Nivel,
                                Dll,
                                Descripcion,
                                Fullname,
                                Name,
                                NameSpace,
                                Parametros
                );
        }

        public string TextoSinSaltosDeLinea
        {
            get
            {
                return FormatearExcepcion("\t");
            }
        }
        public string TextoConSaltosDeLinea
        {
            get
            {
                return FormatearExcepcion("\n");
            }
        }
        public string TextoConSaltosDeLineaTabulados
        {
            get
            {
                return FormatearExcepcion("\n\t");
            }
        }
    }
}
