

namespace Ejercicio.Utilities.Extensions
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
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
