
namespace Ejercicio.Trazas
{
    using Ejercicio.Utilities;
    using Castle.DynamicProxy;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TrazaLoggerInterceptor : IInterceptor
    {
        public static string TRAZA_0 = "traza-0";
        public static string TRAZA_1 = "traza-1";
        public static string TRAZA_2 = "traza-2";
        public static string TRAZA_3 = "traza-3";
        public static string TRAZA_4 = "traza-4";
        public static string TRAZA_5 = "traza-5";
        public static int TRAZA_NIVEL_0 = 0;
        public static int TRAZA_NIVEL_1 = 1;
        public static int TRAZA_NIVEL_2 = 2;
        public static int TRAZA_NIVEL_3 = 3;
        public static int TRAZA_NIVEL_4 = 4;
        public static int TRAZA_NIVEL_5 = 5;

        private int Nivel { get; set; }
        private int NivelConfig { get; set; }

        public IEnumerable<ITrazaLog> TrazasLog { get; set; }
        public IEnumerable<IExceptionLog> ExcepcionesLog { get; set; }

        public TrazaLoggerInterceptor(int nivel, IEnumerable<ITrazaLog> trazasLog, IEnumerable<IExceptionLog> excepcionesLog)
        {
            this.Nivel = nivel;
            this.TrazasLog = trazasLog.ToList();
            this.ExcepcionesLog = excepcionesLog.ToList();
            this.NivelConfig = Utils.Setting<int>("NivelTraza");
        }

        /// <summary>
        /// Constructor para utilizar en los test unitarios
        /// </summary>
        public TrazaLoggerInterceptor()
        {
            this.Nivel = -1;
            this.NivelConfig = -2;
            this.TrazasLog = new List<ITrazaLog>();
            this.ExcepcionesLog = new List<IExceptionLog>();
        }

        public void Intercept(IInvocation invocation)
        {
            CrearTraza(invocation);
            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                var parametros = invocation.Arguments.Select(a => (a ?? "").ToString());

                CrearExcepcion(invocation, ex, parametros);

                throw ex;
            }
        }

        private void CrearExcepcion(IInvocation invocation, Exception ex, IEnumerable<string> parametros)
        {
            Task.Run(() => GuardarExcepcion(
                    Guid.NewGuid(),
                    UsuarioContexto.Usuario,
                    invocation.Method.Module.Name,
                    invocation.Method.DeclaringType.FullName,
                    invocation.Method.DeclaringType.Namespace,
                    invocation.Method.Name,
                    ex,
                    parametros
                    ));
        }

        public void GuardarExcepcion(Guid uid, string nick, string dll, string fullname, string nameSpace, string name, Exception ex, IEnumerable<string> parametros = null)
        {
            try
            {
                var excepcion = new Traza()
                {
                    Uid = uid,
                    UidPeticion = UsuarioContexto.UidPeticion,
                    EsExcepcion = true,
                    Nick = nick,
                    Dll = dll,
                    Fullname = fullname,
                    NameSpace = nameSpace,
                    Name = name,
                    Descripcion = FlattenException(ex),
                    Parametros = FlattenParam(parametros)
                };

                ExcepcionesLog.ToList().ForEach(exl =>
                {
                    try
                    {
                        exl.SalvarLog(excepcion);
                    }
                    catch
                    {

                    }
                });
            }
            catch (Exception exc)
            {
                Debug.WriteLine("EL GUARDADO DE LA EXCEPCIÓN HA GENERADO UN ERROR. " + FlattenException(exc));
                return;
            }
        }

        private static string FlattenParam(IEnumerable<string> parametros)
        {
            if (parametros == null) return string.Empty;
            return string.Join(",", parametros.Select(a => (a ?? "[Null]").ToString()));
        }
        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine("---------------------------------------------------------");
                stringBuilder.AppendLine("---Message");
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine("---StackTrace");
                stringBuilder.AppendLine(exception.StackTrace);
                stringBuilder.AppendLine("---Source");
                stringBuilder.AppendLine(exception.Source);
                stringBuilder.AppendLine("---Data");
                foreach (var key in exception.Data.Keys)
                {
                    stringBuilder.AppendLine(key + " -> " + exception.Data[key].ToString());
                }
                stringBuilder.AppendLine("---------------------------------------------------------");

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        private void CrearTraza(IInvocation invocation)
        {
            var parametros = SerializeParams(invocation);
            Task.Run(() =>
                GuardarTraza(
                    Guid.NewGuid(),
                    UsuarioContexto.Usuario,
                    Nivel,
                    invocation.Method.Module.Name,
                    invocation.Method.DeclaringType.FullName,
                    invocation.Method.DeclaringType.Namespace,
                    invocation.Method.Name,
                    parametros)
             );
        }

        private string SerializeParams(IInvocation invocation)
        {
            var parametros = invocation.Arguments.Select(a => a == null ? "null" : (a.GetType().IsSubclassOf(typeof(System.IO.Stream)) ? "stream" : JsonConvert.SerializeObject(a)));
            return string.Join(",", parametros);
        }

        public void GuardarTraza(Guid uid, string nick, int Nivel, string dll, string fullname, string nameSpace, string name, string parametros)
        {
            if (NivelConfig >= Nivel)
            {
                try
                {
                    var traza = new Traza()
                    {
                        Uid = uid,
                        UidPeticion = UsuarioContexto.UidPeticion,
                        EsExcepcion = false,
                        Nick = nick,
                        Dll = dll,
                        Fullname = fullname,
                        NameSpace = nameSpace,
                        Name = name,
                        Descripcion = String.Empty,
                        Parametros = parametros,
                        CreadoEn = DateTime.UtcNow,
                        Nivel = Nivel
                    };

                    TrazasLog.ToList().ForEach(tl =>
                    {
                        try
                        {
                            tl.SalvarLog(traza);
                        }
                        catch
                        {

                        }
                    });

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EL GUARDADO DE LA TRAZA HA GENERADO UN ERROR. " + ex.ToString());

                    return;
                }
            }
        }
    }
}
