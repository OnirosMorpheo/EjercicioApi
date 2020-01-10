using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejercicio.Utilities.Extensions
{
    public static class FormDictionaryExtension
    {        
        private static Dictionary<string, string> ObjectToFormDictionary(this object doc)
        {
            string jsonData = JsonConvert.SerializeObject(doc);

            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
            dict = dict.Where(kv => kv.Value != null).ToDictionary(kv => kv.Key, kv => kv.Value);
            return dict;
        }
    }
}
