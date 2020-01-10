

namespace Ejercicio.WebApi
{
    using Ejercicio.Trazas;
    using Ejercicio.Utilities;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// 
    /// </summary>
    internal class TokenValidationHandler : DelegatingHandler
    {
        public TokenValidationHandler()
        {

        }

        private bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            string bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase) ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            SecurityToken securityToken;
            string token;

            // determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                statusCode = HttpStatusCode.Unauthorized;
                return base.SendAsync(request, cancellationToken);
            }

            try
            {
                string secretKey = Utils.Setting<string>("JWT_SECRET_KEY");
                string audienceToken = Utils.Setting<string>("JWT_AUDIENCE_TOKEN");
                string issuerToken = Utils.Setting<string>("JWT_ISSUER_TOKEN");
                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                TokenValidationParameters validationParameters = new TokenValidationParameters()
                {
                    ValidAudience = audienceToken,
                    ValidIssuer = issuerToken,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    LifetimeValidator = this.LifetimeValidator,
                    IssuerSigningKey = securityKey
                };

                // Extract and assign Current Principal and user
                /*IMPERSONAR NOMBRE USUARIO SOLO DISPONIBLE PARA APLICACIONES AUTORIZADAS*/
                // Asigna el ClaimsPrincipal.
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                Thread.CurrentPrincipal = claimsPrincipal;
                HttpContext.Current.User = claimsPrincipal;

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
                var logger = DependencyResolver.Current.GetService<TrazaLoggerInterceptor>();
                logger.GuardarExcepcion(Guid.NewGuid(), UsuarioContexto.Nombre, "WebApi", ex.GetType().FullName, "", "TokenValidationHandler", ex);
            }

            return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {

            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}