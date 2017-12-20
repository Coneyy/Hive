using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HiveAPI.Security
{
    public static class JWTTokenCreator
    {
        public static JwtSecurityToken GetNewToken(string name,string key, int time)
        {

            var claims = new[]
            {
                new Claim("Id", name)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "hivegameapi.azurewebsites.net",
                audience: "hivegameapi.azurewebsites.net",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds);

            return token;
        }
    }
}
