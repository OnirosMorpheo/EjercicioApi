
namespace Ejercicio.Trazas
{
    using Ejercicio.Utilities;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    public class EmailLog : ITrazaLog, IExceptionLog
    {
        public bool SalvarLog(Traza traza)
        {
            try
            {
                
                SmtpClient cliente = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("kepa.pedro@gamil.com", "")
                };

                MailMessage mensaje = new MailMessage();
                mensaje.From = new MailAddress(Utils.Setting<string>("FromEmailLog"));
                var emails = Utils.SettingList<string>("ToEmailLog");

                emails.ToList().ForEach(e => mensaje.To.Add(e));

                var tipo = traza.EsExcepcion ? " [EXCEPCION]" : " [TRAZA]";

                mensaje.Subject = Utils.Setting<string>("AsuntoEmailLog") + tipo;
                mensaje.IsBodyHtml = false;
                mensaje.Body = traza.TextoConSaltosDeLinea;

                cliente.Send(mensaje);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


    }
}
