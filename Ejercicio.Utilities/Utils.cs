
namespace Ejercicio.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;

    public class Utils
    {
        private static Configuration _config;
        public static Configuration Config
        {
            get
            {
                if (_config == null)
                {
                    if (HttpRuntime.AppDomainAppId != null)
                        _config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);
                    else
                        _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                return _config;
            }
        }

        public static string ConnectionStrings(string name)
        {
            return Config.ConnectionStrings.ConnectionStrings[name].ConnectionString;
        }

        public static List<T> SettingList<T>(string name)
        {
            string value = Config.AppSettings.Settings[name].Value;
            value = value ?? "";
            var values = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(v => (T)Convert.ChangeType(v.Trim(), typeof(T), CultureInfo.InvariantCulture)).ToList();

            return values;
        }

        public static T Setting<T>(string name)
        {
            string value = Config.AppSettings.Settings[name].Value;
            value = value ?? "";
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T GetClaim<T>(IEnumerable<Claim> claims, string tipoClaim)
        {
            string value = claims.Where(x => x.Type == tipoClaim).Select(c => c.Value).FirstOrDefault();
            value = value ?? "";
            var tipo = typeof(T);

            if (string.IsNullOrEmpty(value))
            {
                if (tipo == typeof(int))
                {
                    return (T)Convert.ChangeType(0, tipo, CultureInfo.InvariantCulture);
                }
                else if (tipo == typeof(bool))
                {
                    return (T)Convert.ChangeType(false, tipo, CultureInfo.InvariantCulture);
                }
                else if (tipo == typeof(Guid))
                {
                    return (T)Convert.ChangeType(Guid.Empty, typeof(T), CultureInfo.InvariantCulture);
                }
                else if (tipo == typeof(Guid?))
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(null);
                }
            }

            if (tipo == typeof(Guid))
            {
                //var guid = new Guid(value);
                return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }

        }
    }
}
