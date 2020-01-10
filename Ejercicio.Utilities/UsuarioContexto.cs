

namespace Ejercicio.Utilities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    public static class UsuarioContexto
    {
        public static int AppId
        {
            get { return Utils.GetClaim<int>(CurrentClaims, CustomClaimTypes.APP_ID); }
        }
        public static Guid AppUid
        {
            get { return Utils.GetClaim<Guid>(CurrentClaims, CustomClaimTypes.APP_UID); }
        }
        public static string Usuario
        {
            get { return Utils.GetClaim<string>(CurrentClaims, CustomClaimTypes.USUARIO); }
        }
        public static Guid Id
        {
            get { return Utils.GetClaim<Guid>(CurrentClaims, CustomClaimTypes.UID_USUARIO); }
        }
        public static Guid UidPeticion
        {
            get { return Utils.GetClaim<Guid>(CurrentClaims, CustomClaimTypes.UID_PETICION); }
        }
        public static string Nombre
        {
            get { return Utils.GetClaim<string>(CurrentClaims, CustomClaimTypes.NOMBRE); }
        }
        public static string Apellidos
        {
            get { return Utils.GetClaim<string>(CurrentClaims, CustomClaimTypes.APELLIDOS); }
        }
        public static string Email
        {
            get { return Utils.GetClaim<string>(CurrentClaims, CustomClaimTypes.EMAIL); }
        }
        public static string Telefono
        {
            get { return Utils.GetClaim<string>(CurrentClaims, CustomClaimTypes.TELEFONO); }
        }
        
        
        public static IEnumerable<Claim> CurrentClaims
        {
            get
            {
                var cp = System.Threading.Thread.CurrentPrincipal;
                return ((ClaimsPrincipal)cp)?.Claims;
            }
        }
    }
}
