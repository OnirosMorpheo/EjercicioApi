

namespace Ejercicio.Models.Api
{
    using Ejercicio.Models.Extensiones;
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel;

    public class FiltroModel
    {
       public int Pagina { get; set; }
        public int TamPag { get; set; } = 25;
        public ListSortDirection Descending { get; set; } = ListSortDirection.Ascending;    //0 => Ascending, 1 => Descending
        [JsonIgnore]
        private string _campo;
        public string CampoOrdenacion { get { return _campo ?? string.Empty; } set { _campo = value; } }
        [JsonConverter(typeof(NullToDefaultConverter<Guid>))]
        public Guid Uid { get; set; }
    }
}
