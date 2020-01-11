

namespace Ejercicio.WebApi
{
    using Ejercicio.Models.Api;
    using Ejercicio.Utilities;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    internal static class TokenGenerator
    {
        public static string GenerateTokenJwt(LoginModel login) {
            string secretKey = Utils.Setting<string>("JWT_SECRET_KEY");
            string audienceToken = Utils.Setting<string>("JWT_AUDIENCE_TOKEN");
            string issuerToken = Utils.Setting<string>("JWT_ISSUER_TOKEN");
            int expireTime = Utils.Setting<int>("JWT_EXPIRE_MINUTES");

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { 
                new Claim(ClaimTypes.Name, "Nombre"),
                new Claim(CustomClaimTypes.APP_ID, Guid.NewGuid().ToString()),
                new Claim(CustomClaimTypes.UID_USUARIO, Guid.NewGuid().ToString()),
                new Claim(CustomClaimTypes.USUARIO, login.User),
                new Claim(CustomClaimTypes.APELLIDOS, "Apellidos"),
                new Claim(CustomClaimTypes.EMAIL, "email"),
                new Claim(CustomClaimTypes.TELEFONO, ""),
                new Claim(CustomClaimTypes.UID_PETICION, Guid.NewGuid().ToString())
            });
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(audienceToken, 
                                                                       issuerToken, 
                                                                       subject: claimsIdentity, 
                                                                       notBefore: DateTime.UtcNow, 
                                                                       expires: DateTime.UtcNow.AddMinutes(expireTime), 
                                                                       signingCredentials: signingCredentials);
            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}