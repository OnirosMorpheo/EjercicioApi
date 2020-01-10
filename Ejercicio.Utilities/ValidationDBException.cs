
namespace Ejercicio.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    public class ValidationDBException : Exception
    {
        public List<string> ListaErrores { get; set; }

        public ValidationDBException(List<string> listaErrores) : base(string.Join("\r\n", listaErrores))
        {
            ListaErrores = listaErrores;
        }

        public static Exception Parse(Exception exception)
        {

            if (exception is SqlException)
            {
                var errores = new List<string>();
                var sqlex = (SqlException)exception;
                //todo crear mensajes de error correctos
                switch (sqlex.Number)
                {
                    case 515:
                        errores.Add("Falta un campo requerido.");
                        break;
                    case 547:
                        errores.Add("Falta un campo FK o una validación ha fallado.");
                        break;
                    case 8152:
                        errores.Add("Longitud de texto superada.");
                        break;
                    default:
                        errores.Add(sqlex.Message);
                        break;
                }
                return new ValidationDBException(errores);
            }
            else
            {
                return null;
            }
        }
    }
}
